using BaskIt.Services.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BaskIt.Commands.Basket.RemoveProduct;

public class RemoveProductFromBasketCommandHandler : IRequestHandler<RemoveProductFromBasketCommand>
{
    private readonly IBasketService basketService;
    private readonly ILogger<RemoveProductFromBasketCommandHandler> logger;

    public RemoveProductFromBasketCommandHandler(
        IBasketService basketService,
        ILogger<RemoveProductFromBasketCommandHandler> logger)
    {
        this.basketService = basketService;
        this.logger = logger;
    }

    public async Task Handle(RemoveProductFromBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await basketService.RemoveProductFromBasketAsync(request.BasketId, request.ProductId, cancellationToken);

            logger.LogInformation("Successfully removed product {ProductId} from basket {BasketId}",
                request.ProductId, request.BasketId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while removing product {ProductId} from basket {BasketId}",
                request.ProductId, request.BasketId);
            throw;
        }
    }
}
