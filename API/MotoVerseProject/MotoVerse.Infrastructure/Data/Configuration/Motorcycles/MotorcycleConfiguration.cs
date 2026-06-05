using MotoVerse.Entities.Models.Motorcycles;

namespace MotoVerse.Infrastructure.Data.Configuration.Motorcycles;

internal class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
{
    public void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        builder.Property(m => m.PricePerDay)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(o => o.Owner)
            .WithMany(m => m.Motorcycles);

        builder.HasMany(r => r.ReviewMotorCycles)
            .WithOne(m => m.Motorcycle)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
