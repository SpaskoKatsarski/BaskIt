using AngleSharp.Dom;
using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Services.Scrape.ProductScraper.Strategies;

/// <summary>
/// Fallback strategy that tries common HTML selectors (lowest priority).
/// Used when no structured data is available.
/// </summary>
public class GenericHtmlProductStrategy : IProductScraperStrategy
{
    public int Priority => 10;

    public bool CanHandle(IDocument document, string? url)
    {
        var name = document.QuerySelector("h1.product-title")?.TextContent?.Trim()
                   ?? document.QuerySelector("h1.product-name")?.TextContent?.Trim()
                   ?? document.QuerySelector(".product-title")?.TextContent?.Trim()
                   ?? document.QuerySelector(".product-name")?.TextContent?.Trim()
                   ?? document.QuerySelector("h1")?.TextContent?.Trim();

        return !string.IsNullOrWhiteSpace(name);
    }

    public Task<ProductScrapedDto?> Extract(IDocument document, string sourceUrl)
    {
        var name = ExtractName(document);
        if (string.IsNullOrWhiteSpace(name))
            return Task.FromResult<ProductScrapedDto?>(null);

        var price = ExtractPrice(document);
        var description = ExtractDescription(document);
        var color = ExtractColor(document);
        var size = ExtractSize(document);
        var imageUrl = ExtractImageUrl(document);

        return Task.FromResult<ProductScrapedDto?>(new ProductScrapedDto
        {
            Name = name,
            Price = price,
            WebsiteUrl = sourceUrl,
            Description = description,
            Color = color,
            Size = size,
            ImageUrl = imageUrl
        });
    }

    private string? ExtractName(IDocument document)
    {
        // Try common selectors for product name
        var name = document.QuerySelector("h1.product-title")?.TextContent?.Trim()
                   ?? document.QuerySelector("h1.product-name")?.TextContent?.Trim()
                   ?? document.QuerySelector(".product-title")?.TextContent?.Trim()
                   ?? document.QuerySelector(".product-name")?.TextContent?.Trim()
                   ?? document.QuerySelector("h1")?.TextContent?.Trim();

        return name;
    }

    private decimal ExtractPrice(IDocument document)
    {
        // Try common selectors for price
        var priceText = document.QuerySelector(".price")?.TextContent?.Trim()
                        ?? document.QuerySelector(".product-price")?.TextContent?.Trim()
                        ?? document.QuerySelector("[class*='price']")?.TextContent?.Trim()
                        ?? "0";

        // Clean and parse price (remove currency symbols, commas, etc.)
        var cleanPrice = new string(priceText.Where(c => char.IsDigit(c) || c == '.').ToArray());
        decimal.TryParse(cleanPrice, out var price);

        return price;
    }

    private string? ExtractDescription(IDocument document)
    {
        // Try common selectors for description
        var description = document.QuerySelector(".product-description")?.TextContent?.Trim()
                          ?? document.QuerySelector(".description")?.TextContent?.Trim()
                          ?? document.QuerySelector("meta[name='description']")?.GetAttribute("content");

        return description;
    }

    private string? ExtractColor(IDocument document)
    {
        // Try common selectors for color
        var color = document.QuerySelector(".product-color")?.TextContent?.Trim()
                    ?? document.QuerySelector("[data-color]")?.GetAttribute("data-color")
                    ?? document.QuerySelector(".color")?.TextContent?.Trim();

        return color;
    }

    private string? ExtractSize(IDocument document)
    {
        // Try common selectors for size
        var size = document.QuerySelector(".product-size")?.TextContent?.Trim()
                   ?? document.QuerySelector("[data-size]")?.GetAttribute("data-size")
                   ?? document.QuerySelector(".size")?.TextContent?.Trim();

        return size;
    }

    private string? ExtractImageUrl(IDocument document)
    {
        // Try common selectors for product image
        var imageElem = document.QuerySelector(".product-image")
                        ?? document.QuerySelector(".main-image")
                        ?? document.QuerySelector("[data-product-image]")
                        ?? document.QuerySelector(".product img")
                        ?? document.QuerySelector("img[alt*='product']")
                        // Fallback to og:image if available
                        ?? document.QuerySelector("meta[property='og:image']");

        if (imageElem != null)
        {
            // Try different attributes where image URL might be
            return imageElem.GetAttribute("src")
                   ?? imageElem.GetAttribute("data-src")
                   ?? imageElem.GetAttribute("data-lazy")
                   ?? imageElem.GetAttribute("content"); // For meta tags
        }

        return null;
    }
}
