using BaskIt.Shared.DTOs.Basket;
using MediatR;

namespace BaskIt.Commands.Basket.GetBasketById;

public record GetBasketByIdQuery(string BasketId) : IRequest<BasketDetailsDto?>;
