using Microsoft.AspNetCore.Identity;

namespace BaskIt.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? TelegramUserId { get; set; }

    public ICollection<Basket> Baskets { get; set; } = new HashSet<Basket>();
}
