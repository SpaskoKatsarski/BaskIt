using BaskIt.Shared.DTOs.Product;
using MediatR;

namespace BaskIt.Commands.Basket.AddProduct;

public record AddProductToBasketCommand(string BasketId, ProductScrapedDto Product) : IRequest;
