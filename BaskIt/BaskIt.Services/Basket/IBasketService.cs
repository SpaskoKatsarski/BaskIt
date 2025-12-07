using BaskIt.Shared.DTOs.Basket;
using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Services.Basket;

public interface IBasketService
{
    Task<string> CreateBasketAsync(string userId, CreateBasketRequest request, CancellationToken cancellationToken = default);

    Task<BasketDetailsDto?> GetBasketByIdAsync(string basketId, CancellationToken cancellationToken = default);

    Task<IEnumerable<BasketListDto>> GetUserBasketsAsync(string userId, CancellationToken cancellationToken = default);

    Task UpdateBasketAsync(string basketId, UpdateBasketRequest request, CancellationToken cancellationToken = default);

    Task DeleteBasketAsync(string basketId, CancellationToken cancellationToken = default);

    Task AddProductToBasketAsync(string basketId, ProductScrapedDto productDto, CancellationToken cancellationToken = default);

    Task RemoveProductFromBasketAsync(string basketId, string productId, CancellationToken cancellationToken = default);
}
