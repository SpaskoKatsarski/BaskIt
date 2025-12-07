using BaskIt.Shared.DTOs.Basket;
using MediatR;

namespace BaskIt.Commands.Basket.GetUserBaskets;

public record GetUserBasketsQuery(string UserId) : IRequest<IEnumerable<BasketListDto>>;
