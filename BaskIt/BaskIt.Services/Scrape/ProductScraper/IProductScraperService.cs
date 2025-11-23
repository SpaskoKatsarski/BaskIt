using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Services.Scrape.ProductScraper;

// Implement AngleSharp into here, inject web content fetcher to get HTML from URL in the second method
public interface IProductScraperService
{
    Task<ProductScrapedDto?> ScrapeProductAsync(string html, string sourceUrl);

    Task<ProductScrapedDto?> ScrapeProductFromUrlAsync(string sourceUrl, CancellationToken ct = default);
}
