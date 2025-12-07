using BaskIt.Shared.DTOs.Basket;
using MediatR;

namespace BaskIt.Commands.Basket.Create;

public record CreateBasketCommand(string UserId, CreateBasketRequest Request) : IRequest<string>;
