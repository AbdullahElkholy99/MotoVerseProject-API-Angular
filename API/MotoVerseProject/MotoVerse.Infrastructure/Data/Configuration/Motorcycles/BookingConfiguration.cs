using MotoVerse.Entities.Models.Motorcycles;

namespace MotoVerse.Infrastructure.Data.Configuration.Motorcycles;

internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasOne(b => b.Payment)
            .WithOne(p => p.Booking)
            .HasForeignKey<Payment>(p => p.BookingId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Motorcycle)
            .WithMany(p => p.Bookings)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(b => b.TotalPrice)
            .HasColumnType("decimal(18,2)");
    }
}
