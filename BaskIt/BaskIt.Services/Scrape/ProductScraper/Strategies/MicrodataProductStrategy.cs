using AngleSharp.Dom;
using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Services.Scrape.ProductScraper.Strategies;

/// <summary>
/// Extracts product data from Schema.org Microdata (medium priority).
/// Uses itemtype, itemprop attributes embedded in HTML.
/// </summary>
public class MicrodataProductStrategy : IProductScraperStrategy
{
    public int Priority => 60;

    public bool CanHandle(IDocument document, string? url)
    {
        return document.QuerySelector("[itemtype*='schema.org/Product']") != null;
    }

    public Task<ProductScrapedDto?> Extract(IDocument document, string sourceUrl)
    {
        var productElem = document.QuerySelector("[itemtype*='schema.org/Product']");
        if (productElem == null)
            return Task.FromResult<ProductScrapedDto?>(null);

        var name = productElem.QuerySelector("[itemprop='name']")?.TextContent?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return Task.FromResult<ProductScrapedDto?>(null);

        var price = ExtractPrice(productElem);
        var description = productElem.QuerySelector("[itemprop='description']")?.TextContent?.Trim();
        var color = ExtractColor(productElem);
        var size = ExtractSize(productElem);
        var imageUrl = ExtractImageUrl(productElem);

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

    private decimal ExtractPrice(IElement productElem)
    {
        // Try offers > price
        var priceElem = productElem.QuerySelector("[itemprop='offers'] [itemprop='price']")
                        ?? productElem.QuerySelector("[itemprop='price']");

        if (priceElem != null)
        {
            // Price can be in content attribute or text content
            var priceStr = priceElem.GetAttribute("content") ?? priceElem.TextContent;

            if (!string.IsNullOrWhiteSpace(priceStr))
            {
                // Clean price string
                var cleanPrice = new string(priceStr.Where(c => char.IsDigit(c) || c == '.').ToArray());
                if (decimal.TryParse(cleanPrice, out var price))
                    return price;
            }
        }

        return 0;
    }

    private string? ExtractColor(IElement productElem)
    {
        var colorElem = productElem.QuerySelector("[itemprop='color']");
        if (colorElem != null)
        {
            return colorElem.GetAttribute("content") ?? colorElem.TextContent?.Trim();
        }

        return null;
    }

    private string? ExtractSize(IElement productElem)
    {
        var sizeElem = productElem.QuerySelector("[itemprop='size']");
        if (sizeElem != null)
        {
            return sizeElem.GetAttribute("content") ?? sizeElem.TextContent?.Trim();
        }

        return null;
    }

    private string? ExtractImageUrl(IElement productElem)
    {
        var imageElem = productElem.QuerySelector("[itemprop='image']");
        if (imageElem != null)
        {
            // Image can be in content attribute, src attribute, or href
            return imageElem.GetAttribute("content")
                   ?? imageElem.GetAttribute("src")
                   ?? imageElem.GetAttribute("href");
        }

        return null;
    }
}
