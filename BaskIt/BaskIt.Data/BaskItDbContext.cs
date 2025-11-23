using BaskIt.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaskIt.Data;

public class BaskItDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public BaskItDbContext(DbContextOptions<BaskItDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<Basket> Baskets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
