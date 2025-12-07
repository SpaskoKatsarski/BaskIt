using BaskIt.Domain.Common;

namespace BaskIt.Domain.Entities;

public class Basket : BaseEntity
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}
