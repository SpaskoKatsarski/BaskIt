namespace BaskIt.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string WebsiteUrl { get; set; } = null!;

    public string? Size { get; set; }

    public string? Color { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public Guid BasketId { get; set; }

    public Basket Basket { get; set; } = null!;
}
