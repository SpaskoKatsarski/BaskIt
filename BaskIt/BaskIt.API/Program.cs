using BaskIt.API.Middleware;
using BaskIt.Data;
using BaskIt.Data.Common.Repository;
using BaskIt.Domain.Entities;
using BaskIt.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Scalar.AspNetCore;
using static BaskIt.Shared.Constants.ApplicationConstants;
using System.Threading.RateLimiting;
using BaskIt.Services.Scrape.WebFetcher;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<BaskItDbContext>(connectionName: DatabaseName);

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<BaskItDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddMediatR(cfg =>
{
    // Reference marker classes.
    cfg.RegisterServicesFromAssemblies(
        typeof(BaskIt.Commands.AssemblyReference).Assembly,
        typeof(BaskIt.Queries.AssemblyReference).Assembly
    );
});

builder.Services.AddSingleton<RateLimiter>(_ => new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
{
    PermitLimit = 10,
    Window = TimeSpan.FromSeconds(1),
    SegmentsPerWindow = 2,
    QueueLimit = 5
}));

builder.Services.AddHttpClient("WebScraper", client =>
{
    // Chrome user-agent to avoid bot detection
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
    client.Timeout = TimeSpan.FromSeconds(30);
})
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        AutomaticDecompression = System.Net.DecompressionMethods.All
    })
    .AddStandardResilienceHandler();

builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IWebContentFetcher, WebContentFetcher>();

// Add exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<BaskItDbContext>();
    await db.Database.MigrateAsync();
}

app.MapDefaultEndpoints();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
