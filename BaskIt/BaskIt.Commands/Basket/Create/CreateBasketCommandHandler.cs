using BaskIt.Services.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BaskIt.Commands.Basket.Create;

public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, string>
{
    private readonly IBasketService basketService;
    private readonly ILogger<CreateBasketCommandHandler> logger;

    public CreateBasketCommandHandler(
        IBasketService basketService,
        ILogger<CreateBasketCommandHandler> logger)
    {
        this.basketService = basketService;
        this.logger = logger;
    }

    public async Task<string> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var basketId = await basketService.CreateBasketAsync(request.UserId, request.Request, cancellationToken);

            logger.LogInformation("Successfully created basket {BasketId} for user {UserId}", basketId, request.UserId);

            return basketId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating basket for user {UserId}", request.UserId);
            throw;
        }
    }
}
