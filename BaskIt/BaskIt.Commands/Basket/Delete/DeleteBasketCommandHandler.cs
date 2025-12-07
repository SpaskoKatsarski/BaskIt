using BaskIt.Services.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BaskIt.Commands.Basket.Delete;

public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand>
{
    private readonly IBasketService basketService;
    private readonly ILogger<DeleteBasketCommandHandler> logger;

    public DeleteBasketCommandHandler(
        IBasketService basketService,
        ILogger<DeleteBasketCommandHandler> logger)
    {
        this.basketService = basketService;
        this.logger = logger;
    }

    public async Task Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await basketService.DeleteBasketAsync(request.BasketId, cancellationToken);

            logger.LogInformation("Successfully deleted basket {BasketId}", request.BasketId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting basket {BasketId}", request.BasketId);
            throw;
        }
    }
}
