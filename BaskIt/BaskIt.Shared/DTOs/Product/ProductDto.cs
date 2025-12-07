namespace BaskIt.Shared.DTOs.Product;

public record ProductDto(
    Guid Id,
    string? Name,
    decimal? Price,
    string? WebsiteUrl,
    string? ImageUrl = null,
    string? Size = null,
    string? Color = null,
    string? Description = null
);
