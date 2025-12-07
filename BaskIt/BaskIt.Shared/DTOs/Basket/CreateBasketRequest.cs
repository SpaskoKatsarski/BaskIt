namespace BaskIt.Shared.DTOs.Basket;

public record CreateBasketRequest(
    string Name,
    string? Description = null
);
