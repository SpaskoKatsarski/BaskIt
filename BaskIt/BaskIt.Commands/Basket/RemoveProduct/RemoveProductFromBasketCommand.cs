using MediatR;

namespace BaskIt.Commands.Basket.RemoveProduct;

public record RemoveProductFromBasketCommand(string BasketId, string ProductId) : IRequest;
