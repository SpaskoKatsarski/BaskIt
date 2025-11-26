using System.Net;
using System.Threading.RateLimiting;

namespace BaskIt.Services.Scrape.WebFetcher;

public class WebContentFetcher : IWebContentFetcher
{
    private readonly HttpClient httpClient;
    private readonly RateLimiter rateLimiter;

    public WebContentFetcher(IHttpClientFactory clientFactory, RateLimiter rateLimiter)
    {
        this.httpClient = clientFactory.CreateClient("WebScraper");
        this.rateLimiter = rateLimiter;
    }

    public async Task<string> FetchHtmlAsync(string url, CancellationToken ct = default)
    {
        using var lease = await this.rateLimiter.AcquireAsync(1, ct);

        if (!lease.IsAcquired)
        {
            throw new InvalidOperationException("Rate limit exceeded. Unable to fetch content.");
        }

        try
        {
            var response = await this.httpClient.GetAsync(url, ct);

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new HttpRequestException("Rate limited by the target website", null, HttpStatusCode.TooManyRequests);
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(ct);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to fetch HTML from {url}", ex);
        }
    }
}
