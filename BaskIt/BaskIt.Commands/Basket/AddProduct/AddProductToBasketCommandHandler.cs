using BaskIt.Services.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BaskIt.Commands.Basket.AddProduct;

public class AddProductToBasketCommandHandler : IRequestHandler<AddProductToBasketCommand>
{
    private readonly IBasketService basketService;
    private readonly ILogger<AddProductToBasketCommandHandler> logger;

    public AddProductToBasketCommandHandler(
        IBasketService basketService,
        ILogger<AddProductToBasketCommandHandler> logger)
    {
        this.basketService = basketService;
        this.logger = logger;
    }

    public async Task Handle(AddProductToBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await basketService.AddProductToBasketAsync(request.BasketId, request.Product, cancellationToken);

            logger.LogInformation("Successfully added product {ProductName} to basket {BasketId}",
                request.Product.Name, request.BasketId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while adding product to basket {BasketId}", request.BasketId);
            throw;
        }
    }
}
