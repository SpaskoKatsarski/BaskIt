using BaskIt.Services.Scrape.ProductScraper;
using BaskIt.Shared.DTOs.Product;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BaskIt.Commands.Product.Add;

public class ProductScrapeCommandHandler : IRequestHandler<ProductScrapeCommand, ProductScrapedDto?>
{
    private readonly IProductScraperService productScraperService;
    private readonly ILogger<ProductScrapeCommandHandler> logger;

    public ProductScrapeCommandHandler(
        IProductScraperService productScraperService,
        ILogger<ProductScrapeCommandHandler> logger)
    {
        this.productScraperService = productScraperService;
        this.logger = logger;
    }

    public async Task<ProductScrapedDto?> Handle(ProductScrapeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await this.productScraperService.ScrapeProductFromUrlAsync(request.PageUrl, cancellationToken);

            if (product != null)
            {
                logger.LogInformation("Successfully scraped product: {ProductName} (Price: {Price}) from {Url}",
                    product.Name, product.Price, request.PageUrl);
            }
            else
            {
                logger.LogWarning("Failed to scrape product from URL: {Url}. No product data could be extracted.", request.PageUrl);
            }

            return product;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while scraping product from URL: {Url}", request.PageUrl);
            throw;
        }
    }
}
