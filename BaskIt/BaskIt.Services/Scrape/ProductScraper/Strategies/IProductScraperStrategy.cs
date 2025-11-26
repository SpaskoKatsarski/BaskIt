using AngleSharp.Dom;
using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Services.Scrape.ProductScraper.Strategies;

public interface IProductScraperStrategy
{
    /// <summary>
    /// Priority of this strategy (higher = checked first).
    /// 100 = Structured data (JSON-LD), 80 = OpenGraph, 60 = Microdata, 10 = Generic HTML
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Determines if this strategy can handle the given document.
    /// </summary>
    bool CanHandle(IDocument document, string? url);

    /// <summary>
    /// Extracts product data from the document.
    /// </summary>
    ProductScrapedDto? Extract(IDocument document, string sourceUrl);
}
