using BaskIt.Shared.DTOs.Product;
using MediatR;

namespace BaskIt.Commands.Product.Add;

// TODO: Change this with ProductDto (with ID) after implementing the database adding logic (create product and add to basket)
public record ProductAddCommand(string PageUrl) : IRequest<ProductScrapedDto?>;
