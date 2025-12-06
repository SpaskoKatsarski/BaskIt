using BaskIt.Shared.DTOs.Product;
using MediatR;

namespace BaskIt.Commands.Product.Add;

public record ProductScrapeCommand(string PageUrl) : IRequest<ProductScrapedDto?>;
