using Microsoft.AspNetCore.Identity;

namespace BaskIt.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? TelegramUserId { get; set; }

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}
