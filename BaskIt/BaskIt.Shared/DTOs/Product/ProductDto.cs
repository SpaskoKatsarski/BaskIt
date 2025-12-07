namespace BaskIt.Shared.DTOs.Product;

public class ProductDto
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? ImageUrl { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public string? Description { get; set; }
}
