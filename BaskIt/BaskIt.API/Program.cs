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

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<BaskItDbContext>(connectionName: DatabaseName);

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    // Password settings (customize as needed)
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<BaskItDbContext>()
.AddDefaultTokenProviders();

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourdomain.com",
            ValidAudience = "yourdomain.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key"))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Add exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<BaskItDbContext>();
    await db.Database.MigrateAsync();
}

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
