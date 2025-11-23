namespace BaskIt.Services.Scrape.WebFetcher;

// TODO: Make this fetcher handle HTTP concerns, retries, rate limiting

public interface IWebContentFetcher
{
    Task<string> FetchHtmlAsync(string url, CancellationToken ct = default);
}
