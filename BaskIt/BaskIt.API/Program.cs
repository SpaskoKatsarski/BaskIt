using BaskIt.API.Extensions;
using BaskIt.API.Middleware;
using BaskIt.Data;
using BaskIt.Data.Common.Repository;
using BaskIt.Domain.Entities;
using BaskIt.Services.Jwt;
using BaskIt.Services.Scrape.ProductScraper;
using BaskIt.Services.Scrape.ProductScraper.Strategies;
using BaskIt.Services.Scrape.WebFetcher;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using static BaskIt.Shared.Constants.ApplicationConstants;

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

builder.Services.AddWebScraperServices();
builder.Services.AddChatClient("gpt-4o-mini", builder.Configuration["OPENAI_API_KEY"] ?? null);

builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSingleton<IWebContentFetcher, WebContentFetcher>();

builder.Services.AddScoped<IProductScraperStrategy, JsonLdProductStrategy>();
builder.Services.AddScoped<IProductScraperStrategy, OpenGraphProductStrategy>();
builder.Services.AddScoped<IProductScraperStrategy, MicrodataProductStrategy>();
builder.Services.AddScoped<IProductScraperStrategy, GenericHtmlProductStrategy>();
builder.Services.AddScoped<IProductScraperStrategy, AiProductScraperStrategy>();

builder.Services.AddScoped<IProductScraperService, ProductScraperService>();

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

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
