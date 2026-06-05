using MotoVerse.Entities.Models.Motorcycles;

namespace MotoVerse.Infrastructure.Data.Configuration.Motorcycles;

internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(p => p.Amount)
            .HasColumnType("decimal(18,2)");

    }
}
