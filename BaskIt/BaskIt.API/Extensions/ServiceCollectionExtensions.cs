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

        services.AddHttpClient("WebScraper", client =>
        {
            // Chrome user-agent to avoid bot detection, by using User-Agent
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            client.Timeout = TimeSpan.FromSeconds(30);
        })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                AutomaticDecompression = System.Net.DecompressionMethods.All
            })
            .AddStandardResilienceHandler();

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
}
