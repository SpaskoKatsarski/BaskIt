using BaskIt.Services.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BaskIt.Commands.Basket.Update;

public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand>
{
    private readonly IBasketService basketService;
    private readonly ILogger<UpdateBasketCommandHandler> logger;

    public UpdateBasketCommandHandler(
        IBasketService basketService,
        ILogger<UpdateBasketCommandHandler> logger)
    {
        this.basketService = basketService;
        this.logger = logger;
    }

    public async Task Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await basketService.UpdateBasketAsync(request.BasketId, request.Request, cancellationToken);

            logger.LogInformation("Successfully updated basket {BasketId}", request.BasketId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating basket {BasketId}", request.BasketId);
            throw;
        }
    }
}
