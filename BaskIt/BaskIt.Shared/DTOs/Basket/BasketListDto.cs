namespace BaskIt.Shared.DTOs.Basket;

public record BasketListDto(
    Guid Id,
    string Name,
    string? Description,
    int ProductCount,
    string? ThumbnailUrl,
    DateTime CreatedAt
);
