using System.Text.Json;
using AngleSharp.Dom;
using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Services.Scrape.ProductScraper.Strategies;

/// <summary>
/// Extracts product data from JSON-LD structured data (highest priority).
/// JSON-LD is the most reliable as it's meant for machine reading.
/// </summary>
public class JsonLdProductStrategy : IProductScraperStrategy
{
    public int Priority => 100;

    public bool CanHandle(IDocument document, string? url)
    {
        var scripts = document.QuerySelectorAll("script[type='application/ld+json']");
        return scripts.Any(s =>
        {
            var content = s.TextContent;
            return !string.IsNullOrWhiteSpace(content) &&
                   (content.Contains("\"@type\":\"Product\"", StringComparison.OrdinalIgnoreCase) ||
                    content.Contains("\"@type\": \"Product\"", StringComparison.OrdinalIgnoreCase));
        });
    }

    public Task<ProductScrapedDto?> Extract(IDocument document, string sourceUrl)
    {
        var scripts = document.QuerySelectorAll("script[type='application/ld+json']");

        foreach (var script in scripts)
        {
            try
            {
                var jsonText = script.TextContent;
                if (string.IsNullOrWhiteSpace(jsonText))
                    continue;

                using var jsonDoc = JsonDocument.Parse(jsonText);
                var root = jsonDoc.RootElement;

                // Handle both single objects and arrays
                var product = FindProductInJson(root);
                if (product.HasValue)
                {
                    return Task.FromResult<ProductScrapedDto?>(ExtractFromJsonElement(product.Value, sourceUrl));
                }
            }
            catch (JsonException)
            {
                // Invalid JSON, continue to next script
                continue;
            }
        }

        return Task.FromResult<ProductScrapedDto?>(null);
    }

    private JsonElement? FindProductInJson(JsonElement element)
    {
        // Direct product
        if (element.ValueKind == JsonValueKind.Object &&
            element.TryGetProperty("@type", out var type) &&
            type.GetString()?.Equals("Product", StringComparison.OrdinalIgnoreCase) == true)
        {
            return element;
        }

        // ProductGroup with hasVariant (common pattern for products with multiple variants)
        if (element.ValueKind == JsonValueKind.Object &&
            element.TryGetProperty("@type", out var typeGroup) &&
            typeGroup.GetString()?.Equals("ProductGroup", StringComparison.OrdinalIgnoreCase) == true &&
            element.TryGetProperty("hasVariant", out var hasVariant))
        {
            // Return the first variant (Product)
            return FindProductInJson(hasVariant);
        }

        // Array of items
        if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                var found = FindProductInJson(item);
                if (found.HasValue)
                    return found;
            }
        }

        // Nested graph
        if (element.ValueKind == JsonValueKind.Object &&
            element.TryGetProperty("@graph", out var graph))
        {
            return FindProductInJson(graph);
        }

        return null;
    }

    private ProductScrapedDto ExtractFromJsonElement(JsonElement product, string sourceUrl)
    {
        var name = product.TryGetProperty("name", out var nameElem)
            ? nameElem.GetString()
            : null;

        var price = ExtractPrice(product);
        var description = ExtractDescription(product);
        var color = ExtractColor(product);
        var size = ExtractSize(product);
        var imageUrl = ExtractImageUrl(product);

        return new ProductScrapedDto
        {
            Name = name ?? "Unknown Product",
            Price = price,
            WebsiteUrl = sourceUrl,
            Description = description,
            Color = color,
            Size = size,
            ImageUrl = imageUrl
        };
    }

    private decimal ExtractPrice(JsonElement product)
    {
        // Try offers.price
        if (product.TryGetProperty("offers", out var offers))
        {
            // Handle single offer or array of offers
            var offer = offers.ValueKind == JsonValueKind.Array
                ? offers.EnumerateArray().FirstOrDefault()
                : offers;

            if (offer.ValueKind == JsonValueKind.Object &&
                offer.TryGetProperty("price", out var priceElem))
            {
                // Price can be string or number
                if (priceElem.ValueKind == JsonValueKind.String)
                {
                    var priceStr = priceElem.GetString();
                    if (decimal.TryParse(priceStr, out var parsedPrice))
                        return parsedPrice;
                }
                else if (priceElem.ValueKind == JsonValueKind.Number)
                {
                    return priceElem.GetDecimal();
                }
            }

            // Try lowPrice for aggregate offers
            if (offer.ValueKind == JsonValueKind.Object &&
                offer.TryGetProperty("lowPrice", out var lowPriceElem))
            {
                if (lowPriceElem.ValueKind == JsonValueKind.String)
                {
                    if (decimal.TryParse(lowPriceElem.GetString(), out var parsedPrice))
                        return parsedPrice;
                }
                else if (lowPriceElem.ValueKind == JsonValueKind.Number)
                {
                    return lowPriceElem.GetDecimal();
                }
            }
        }

        return 0;
    }

    private string? ExtractDescription(JsonElement product)
    {
        if (product.TryGetProperty("description", out var desc))
            return desc.GetString();

        return null;
    }

    private string? ExtractColor(JsonElement product)
    {
        if (product.TryGetProperty("color", out var color))
        {
            // Color can be string or object with "name" property
            if (color.ValueKind == JsonValueKind.String)
                return color.GetString();

            if (color.ValueKind == JsonValueKind.Object &&
                color.TryGetProperty("name", out var colorName))
                return colorName.GetString();
        }

        return null;
    }

    private string? ExtractSize(JsonElement product)
    {
        if (product.TryGetProperty("size", out var size))
        {
            if (size.ValueKind == JsonValueKind.String)
                return size.GetString();
        }

        // Try additional properties
        if (product.TryGetProperty("additionalProperty", out var props) &&
            props.ValueKind == JsonValueKind.Array)
        {
            foreach (var prop in props.EnumerateArray())
            {
                if (prop.TryGetProperty("name", out var propName) &&
                    propName.GetString()?.Equals("size", StringComparison.OrdinalIgnoreCase) == true &&
                    prop.TryGetProperty("value", out var value))
                {
                    return value.GetString();
                }
            }
        }

        return null;
    }

    private string? ExtractImageUrl(JsonElement product)
    {
        if (product.TryGetProperty("image", out var image))
        {
            // Image can be string, array of strings, or array of objects
            if (image.ValueKind == JsonValueKind.String)
                return image.GetString();

            if (image.ValueKind == JsonValueKind.Array)
            {
                var firstImage = image.EnumerateArray().FirstOrDefault();

                // Array of strings
                if (firstImage.ValueKind == JsonValueKind.String)
                    return firstImage.GetString();

                // Array of objects with "url" property
                if (firstImage.ValueKind == JsonValueKind.Object &&
                    firstImage.TryGetProperty("url", out var url))
                    return url.GetString();
            }
        }

        return null;
    }
}
