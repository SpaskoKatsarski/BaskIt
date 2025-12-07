using BaskIt.Services.Basket;
using BaskIt.Shared.DTOs.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BaskIt.Commands.Basket.GetBasketById;

public class GetBasketByIdQueryHandler : IRequestHandler<GetBasketByIdQuery, BasketDetailsDto?>
{
    private readonly IBasketService basketService;
    private readonly ILogger<GetBasketByIdQueryHandler> logger;

    public GetBasketByIdQueryHandler(
        IBasketService basketService,
        ILogger<GetBasketByIdQueryHandler> logger)
    {
        this.basketService = basketService;
        this.logger = logger;
    }

    public async Task<BasketDetailsDto?> Handle(GetBasketByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var basket = await basketService.GetBasketByIdAsync(request.BasketId, cancellationToken);

            if (basket != null)
            {
                logger.LogInformation("Successfully retrieved basket {BasketId}", request.BasketId);
            }
            else
            {
                logger.LogWarning("Basket {BasketId} not found", request.BasketId);
            }

            return basket;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving basket {BasketId}", request.BasketId);
            throw;
        }
    }
}
