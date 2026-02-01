using BaskIt.Data.Common.Repository;
using BaskIt.Services.Basket;
using BaskIt.Services.Jwt;
using BaskIt.Services.Scrape.ProductScraper;
using BaskIt.Services.Scrape.ProductScraper.Strategies;
using BaskIt.Services.Scrape.WebFetcher;
using OpenAI.Chat;
using System.Threading.RateLimiting;

namespace BaskIt.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebScraperServices(this IServiceCollection services)
    {
        services.AddSingleton<RateLimiter>(_ => new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromSeconds(1),
            SegmentsPerWindow = 2,
            QueueLimit = 5
        }));

        return services;
    }
    
    public static IServiceCollection AddChatClient(this IServiceCollection services, string model, string? apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentNullException("Cannot register chat client without a provided API key.");
        }

        services.AddSingleton(serviceProvider =>
        {
            return new ChatClient(model, apiKey);
        });

        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IRepository, Repository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddSingleton<IWebContentFetcher, WebContentFetcher>();

        services.AddScoped<IProductScraperStrategy, JsonLdProductStrategy>();
        services.AddScoped<IProductScraperStrategy, OpenGraphProductStrategy>();
        services.AddScoped<IProductScraperStrategy, MicrodataProductStrategy>();
        services.AddScoped<IProductScraperStrategy, GenericHtmlProductStrategy>();
        services.AddScoped<IProductScraperStrategy, AiProductScraperStrategy>();

        services.AddScoped<IProductScraperService, ProductScraperService>();
        services.AddScoped<IBasketService, BasketService>();

        return services;
    }
}
