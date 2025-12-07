using BaskIt.Shared.DTOs.Basket;
using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Services.Basket;

public interface IBasketService
{
    Task<string> CreateBasketAsync(Guid userId, CreateBasketRequest request, CancellationToken cancellationToken = default);

    Task<BasketDetailsDto?> GetBasketByIdAsync(Guid basketId, CancellationToken cancellationToken = default);

    Task<IEnumerable<BasketListDto>> GetUserBasketsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task UpdateBasketAsync(Guid basketId, UpdateBasketRequest request, CancellationToken cancellationToken = default);

    Task DeleteBasketAsync(Guid basketId, CancellationToken cancellationToken = default);

    Task<ProductDto> AddProductToBasketAsync(Guid basketId, ProductScrapedDto product, CancellationToken cancellationToken = default);

    Task RemoveProductFromBasketAsync(Guid basketId, Guid productId, CancellationToken cancellationToken = default);
}
