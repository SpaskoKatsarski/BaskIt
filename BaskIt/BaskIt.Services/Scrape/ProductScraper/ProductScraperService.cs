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

        // Create AngleSharp browsing context
        var config = Configuration.Default;
        this.browsingContext = BrowsingContext.New(config);
    }

    public async Task<ProductScrapedDto?> ScrapeProductAsync(string html, string sourceUrl)
    {
        var document = await this.browsingContext.OpenAsync(req => req.Content(html));

        ProductScrapedDto? mergedProduct = null;

        foreach (var strategy in this.strategies)
        {
            if (strategy.CanHandle(document, sourceUrl))
            {
                var extractedProduct = await strategy.Extract(document, sourceUrl);

                if (extractedProduct != null)
                {
                    MergeProducts(ref mergedProduct, extractedProduct);

                    if (IsProductComplete(mergedProduct))
                        return mergedProduct;
                }
            }
        }

        return mergedProduct;
    }

    public async Task<ProductScrapedDto?> ScrapeProductFromUrlAsync(string sourceUrl, CancellationToken ct = default)
    {
        var html = await this.contentFetcher.FetchHtmlAsync(sourceUrl, ct);

        return await ScrapeProductAsync(html, sourceUrl);
    }

    private void MergeProducts(ref ProductScrapedDto? existing, ProductScrapedDto newData)
    {
        if (existing == null)
        {
            existing = newData;
            return;
        }

        existing.Name = !string.IsNullOrWhiteSpace(existing.Name) ? existing.Name : newData.Name;
        existing.Price = existing.Price ?? newData.Price;
        existing.WebsiteUrl = !string.IsNullOrWhiteSpace(existing.WebsiteUrl) ? existing.WebsiteUrl : newData.WebsiteUrl;
        existing.Size = !string.IsNullOrWhiteSpace(existing.Size) ? existing.Size : newData.Size;
        existing.Color = !string.IsNullOrWhiteSpace(existing.Color) ? existing.Color : newData.Color;
        existing.Description = !string.IsNullOrWhiteSpace(existing.Description) ? existing.Description : newData.Description;
    }

    private bool IsProductComplete(ProductScrapedDto? product)
    {
        return product != null
            && !string.IsNullOrWhiteSpace(product.Name)
            && product.Price.HasValue
            && !string.IsNullOrWhiteSpace(product.WebsiteUrl)
            && !string.IsNullOrWhiteSpace(product.Size)
            && !string.IsNullOrWhiteSpace(product.Color)
            && !string.IsNullOrWhiteSpace(product.Description);
    }
}
