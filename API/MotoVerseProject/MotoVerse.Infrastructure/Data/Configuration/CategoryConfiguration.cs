namespace MotoVerse.Infrastructure.Data.Configuration;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Table name
        builder.ToTable("Categories");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.NameAr)
               .IsRequired()
               .HasMaxLength(100);
        builder.Property(c => c.NameEn)
               .IsRequired()
               .HasMaxLength(100);
        builder.Property(c => c.Description)
               .IsRequired()
               .HasMaxLength(500);
    }
}
