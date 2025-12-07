using BaskIt.Services.Basket;
using BaskIt.Shared.DTOs.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BaskIt.Commands.Basket.GetUserBaskets;

public class GetUserBasketsQueryHandler : IRequestHandler<GetUserBasketsQuery, IEnumerable<BasketListDto>>
{
    private readonly IBasketService basketService;
    private readonly ILogger<GetUserBasketsQueryHandler> logger;

    public GetUserBasketsQueryHandler(
        IBasketService basketService,
        ILogger<GetUserBasketsQueryHandler> logger)
    {
        this.basketService = basketService;
        this.logger = logger;
    }

    public async Task<IEnumerable<BasketListDto>> Handle(GetUserBasketsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var baskets = await basketService.GetUserBasketsAsync(request.UserId, cancellationToken);

            logger.LogInformation("Successfully retrieved {Count} baskets for user {UserId}",
                baskets.Count(), request.UserId);

            return baskets;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving baskets for user {UserId}", request.UserId);
            throw;
        }
    }
}
