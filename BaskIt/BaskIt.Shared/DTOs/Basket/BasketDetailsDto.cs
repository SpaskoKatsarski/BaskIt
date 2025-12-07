using BaskIt.Shared.DTOs.Product;

namespace BaskIt.Shared.DTOs.Basket;

public class BasketDetailsDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public IEnumerable<ProductDto> Products { get; set; } = [];

    public DateTime CreatedAt { get; set; }
}
