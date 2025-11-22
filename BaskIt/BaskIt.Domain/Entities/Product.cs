namespace BaskIt.Domain.Entities;

public class Product
{
    public Guid ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string WebsiteUrl { get; set; } = null!;

    public string? Size { get; set; }

    public string? Color { get; set; }

    public string? Description { get; set; }

    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
