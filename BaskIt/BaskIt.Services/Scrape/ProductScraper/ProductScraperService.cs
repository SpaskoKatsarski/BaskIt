using AngleSharp;
using BaskIt.Services.Scrape.ProductScraper.Strategies;
using BaskIt.Services.Scrape.WebFetcher;
using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Services.Scrape.ProductScraper;

public class ProductScraperService : IProductScraperService
{
    private readonly IWebContentFetcher contentFetcher;
    private readonly IBrowsingContext browsingContext;
    private readonly IEnumerable<IProductScraperStrategy> strategies;

    public ProductScraperService(
        IWebContentFetcher contentFetcher,
        IEnumerable<IProductScraperStrategy> strategies)
    {
        this.contentFetcher = contentFetcher;
        this.strategies = strategies.OrderByDescending(s => s.Priority);

        // Create AngleSharp browsing context (parses HTML)
        var config = Configuration.Default;
        this.browsingContext = BrowsingContext.New(config);
    }

    public async Task<ProductScrapedDto?> ScrapeProductAsync(string html, string sourceUrl)
    {
        var document = await this.browsingContext.OpenAsync(req => req.Content(html));

        foreach (var strategy in this.strategies)
        {
            if (strategy.CanHandle(document, sourceUrl))
            {
                var result = strategy.Extract(document, sourceUrl);

                if (result != null)
                {
                    return result;
                }
            }
        }

        // No strategy could extract product data
        return null;
    }

    public async Task<ProductScrapedDto?> ScrapeProductFromUrlAsync(string sourceUrl, CancellationToken ct = default)
    {
        var html = await this.contentFetcher.FetchHtmlAsync(sourceUrl, ct);

        return await ScrapeProductAsync(html, sourceUrl);
    }
}
