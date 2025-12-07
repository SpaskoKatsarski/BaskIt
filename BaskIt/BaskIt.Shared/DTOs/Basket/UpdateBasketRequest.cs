namespace BaskIt.Shared.DTOs.Basket;

public record UpdateBasketRequest(
    string Name,
    string? Description = null
);
