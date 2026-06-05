namespace MotoVerse.Infrastructure.Data.Configuration;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Table name
        builder.ToTable("Products");

        // Primary Key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.NameAr)
               .IsRequired()
               .HasMaxLength(150);
        builder.Property(p => p.NameEn)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(p => p.Price)
               .IsRequired();

        // Relationship (Product -> Category)
        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
