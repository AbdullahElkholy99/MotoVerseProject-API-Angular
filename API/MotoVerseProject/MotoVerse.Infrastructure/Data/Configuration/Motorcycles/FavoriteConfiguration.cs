using MotoVerse.Entities.Models.Motorcycles;

namespace MotoVerse.Infrastructure.Data.Configuration.Motorcycles;

internal class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasKey(f => f.Id);

        builder.HasIndex(f => new
        {
            f.CustomerId,
            f.MotorcycleId
        }).IsUnique();

        builder.HasOne(f => f.Customer)
            .WithMany()
            .HasForeignKey(f => f.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Motorcycle)
            .WithMany()
            .HasForeignKey(f => f.MotorcycleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

