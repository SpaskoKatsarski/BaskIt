using BaskIt.Shared.DTOs.Basket;
using MediatR;

namespace BaskIt.Commands.Basket.Update;

public record UpdateBasketCommand(string BasketId, UpdateBasketRequest Request) : IRequest;
