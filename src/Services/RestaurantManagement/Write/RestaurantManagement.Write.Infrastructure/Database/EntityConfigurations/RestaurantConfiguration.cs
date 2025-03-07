using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Write.Domain.Models.RestaurantAggregate;

namespace RestaurantManagement.Write.Infrastructure.Database.EntityConfigurations;

public class RestaurantOwnerConfiguration : IEntityTypeConfiguration<RestaurantOwner>
{
    public void Configure(EntityTypeBuilder<RestaurantOwner> builder)
    {
    }
}

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasOne<RestaurantOwner>()
            .WithMany(m => m.Restaurants)
            .HasForeignKey(x => x.OwnerId);

        builder.ComplexProperty(p => p.Address);
        builder.Property(p => p.Status).IsEnum();
    }
}
