using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Services.Scrape.ProductScraper;

public interface IProductScraperService
{
    Task<ProductScrapedDto?> ScrapeProductAsync(string html, string sourceUrl);

    Task<ProductScrapedDto?> ScrapeProductFromUrlAsync(string sourceUrl, CancellationToken ct = default);
}
