using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Shared.DTOs.Basket;

public record BasketDetailsDto(
    Guid Id,
    string Name,
    string? Description,
    IEnumerable<ProductDto> Products,
    DateTime CreatedAt
);
