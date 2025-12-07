using MediatR;

namespace BaskIt.Commands.Basket.Delete;

public record DeleteBasketCommand(string BasketId) : IRequest;
