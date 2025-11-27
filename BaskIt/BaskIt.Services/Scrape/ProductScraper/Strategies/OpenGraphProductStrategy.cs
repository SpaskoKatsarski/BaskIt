using AngleSharp.Dom;
using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Services.Scrape.ProductScraper.Strategies;

/// <summary>
/// Extracts product data from OpenGraph meta tags (high priority).
/// Used by many e-commerce sites for social media sharing.
/// </summary>
public class OpenGraphProductStrategy : IProductScraperStrategy
{
    public int Priority => 80;

    public bool CanHandle(IDocument document, string? url)
    {
        // Check if page has og:type = product or product.item
        var ogType = document.QuerySelector("meta[property='og:type']")?.GetAttribute("content");
        return ogType != null &&
               (ogType.Contains("product", StringComparison.OrdinalIgnoreCase) ||
                ogType.Equals("product.item", StringComparison.OrdinalIgnoreCase));
    }

    public Task<ProductScrapedDto?> Extract(IDocument document, string sourceUrl)
    {
        var name = GetMetaContent(document, "og:title")
                   ?? GetMetaContent(document, "og:product:title");

        if (string.IsNullOrWhiteSpace(name))
            return Task.FromResult<ProductScrapedDto?>(null);

        var price = ExtractPrice(document);
        var description = GetMetaContent(document, "og:description")
                          ?? GetMetaContent(document, "og:product:description");

        var color = GetMetaContent(document, "og:product:color")
                    ?? GetMetaContent(document, "product:color");

        var size = GetMetaContent(document, "og:product:size")
                   ?? GetMetaContent(document, "product:size");

        return Task.FromResult<ProductScrapedDto?>(new ProductScrapedDto
        {
            Name = name,
            Price = price,
            WebsiteUrl = sourceUrl,
            Description = description,
            Color = color,
            Size = size
        });
    }

    private string? GetMetaContent(IDocument document, string property)
    {
        return document.QuerySelector($"meta[property='{property}']")?.GetAttribute("content");
    }

    private decimal ExtractPrice(IDocument document)
    {
        // Try various OpenGraph price properties
        var priceStr = GetMetaContent(document, "og:price:amount")
                       ?? GetMetaContent(document, "product:price:amount")
                       ?? GetMetaContent(document, "og:product:price:amount");

        if (!string.IsNullOrWhiteSpace(priceStr))
        {
            // Clean price string (remove currency symbols, commas)
            var cleanPrice = new string(priceStr.Where(c => char.IsDigit(c) || c == '.').ToArray());
            if (decimal.TryParse(cleanPrice, out var price))
                return price;
        }

        return 0;
    }
}
