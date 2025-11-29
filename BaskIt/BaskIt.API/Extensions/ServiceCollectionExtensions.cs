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
}
