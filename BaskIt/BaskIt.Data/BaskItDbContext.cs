using BaskIt.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaskIt.Data;

public class BaskItDbContext : IdentityDbContext<ApplicationUser>
{
    public BaskItDbContext(DbContextOptions<BaskItDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
