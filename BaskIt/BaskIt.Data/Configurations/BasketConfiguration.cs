using BaskIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaskIt.Data.Configurations;

public class BasketConfiguration : BaseEntityConfiguration<Basket>
{
    public override void Configure(EntityTypeBuilder<Basket> builder)
    {
        base.Configure(builder);

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Description)
            .HasMaxLength(1000);

        builder.HasOne(b => b.User)
            .WithMany(u => u.Baskets)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(b => b.Products)
            .WithOne(p => p.Basket)
            .HasForeignKey(p => p.BasketId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
