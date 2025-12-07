using BaskIt.Data.Common.Repository;
using BaskIt.Domain.Entities;
using BaskIt.Shared.DTOs.Basket;
using BaskIt.Shared.DTOs.Product;
using Microsoft.EntityFrameworkCore;

namespace BaskIt.Services.Basket;

public class BasketService : IBasketService
{
    private readonly IRepository repository;

    public BasketService(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task AddProductToBasketAsync(string basketId, ProductScrapedDto productDto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(productDto.Name))
            throw new ArgumentException("Product name cannot be null or empty.", nameof(productDto.Name));

        var product = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price ?? 0,
            WebsiteUrl = productDto.WebsiteUrl ?? string.Empty,
            Size = productDto.Size,
            Color = productDto.Color,
            Description = productDto.Description,
            ImageUrl = productDto.ImageUrl,
            BasketId = Guid.Parse(basketId)
        };

        await repository.AddAsync(product, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> CreateBasketAsync(string userId, CreateBasketRequest request, CancellationToken cancellationToken = default)
    {
        var userGuid = Guid.Parse(userId);

        var nameTaken = await repository.AllReadOnly<Domain.Entities.Basket>()
            .AnyAsync(b => b.UserId == userGuid && b.Name == request.Name, cancellationToken);

        if (nameTaken)
            throw new ArgumentException("A basket with the same name already exists for this user.", nameof(request.Name));

        var basket = new Domain.Entities.Basket
        {
            Name = request.Name,
            Description = request.Description,
            UserId = userGuid
        };

        await repository.AddAsync(basket, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return basket.Id.ToString();
    }

    public async Task DeleteBasketAsync(string basketId, CancellationToken cancellationToken = default)
    {
        var basket = await repository.GetByIdAsync<Domain.Entities.Basket>(Guid.Parse(basketId), cancellationToken);

        if (basket == null)
            throw new ArgumentException("Basket not found.", nameof(basketId));

        repository.SoftDelete(basket);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<BasketDetailsDto?> GetBasketByIdAsync(string basketId, CancellationToken cancellationToken = default)
    {
        var basket = await repository.AllReadOnly<Domain.Entities.Basket>()
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.Id == Guid.Parse(basketId), cancellationToken);

        return basket == null ? null : new BasketDetailsDto
        {
            Id = basket.Id,
            Name = basket.Name,
            Description = basket.Description,
            CreatedAt = basket.CreatedAt,
            Products = basket.Products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                WebsiteUrl = p.WebsiteUrl,
                Size = p.Size,
                Color = p.Color,
                Description = p.Description,
                ImageUrl = p.ImageUrl
            })
        };
    }

    public async Task<IEnumerable<BasketListDto>> GetUserBasketsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var baskets = await repository.AllReadOnly<Domain.Entities.Basket>()
            .Where(b => b.UserId == Guid.Parse(userId))
            .Select(b => new BasketListDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                ProductCount = b.Products.Count,
                ThumbnailUrl = b.Products
                    .OrderBy(p => p.CreatedAt)
                    .Select(p => p.ImageUrl)
                    .FirstOrDefault(),
                CreatedAt = b.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return baskets;
    }

    public async Task RemoveProductFromBasketAsync(string basketId, string productId, CancellationToken cancellationToken = default)
    {
        var basket = await repository.GetByIdAsync<Domain.Entities.Basket>(Guid.Parse(basketId), cancellationToken);

        if (basket == null)
            throw new ArgumentException("Basket not found.", nameof(basketId));

        var product = await repository.GetByIdAsync<Product>(Guid.Parse(productId), cancellationToken);

        if (product == null)
            throw new ArgumentException("Product not found.", nameof(productId));

        if (product.BasketId != basket.Id)
            throw new InvalidOperationException("Product does not belong to the specified basket.");

        repository.SoftDelete(product);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateBasketAsync(string basketId, UpdateBasketRequest request, CancellationToken cancellationToken = default)
    {
        var basket = await repository.GetByIdAsync<Domain.Entities.Basket>(Guid.Parse(basketId), cancellationToken);

        if (basket == null)
            throw new ArgumentException("Basket not found.", nameof(basketId));

        basket.Name = request.Name;
        basket.Description = request.Description;

        await repository.SaveChangesAsync(cancellationToken);
    }
}
