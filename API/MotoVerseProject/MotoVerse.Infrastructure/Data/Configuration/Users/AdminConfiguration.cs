
namespace MotoVerse.Infrastructure.Data.Configuration.Users;

internal class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        // Table name
        builder.ToTable("Admins");

        // relation with product
        builder.HasMany(a => a.Products)
            .WithOne(p => p.Admin)
            .HasForeignKey(p => p.AdminId)
            .OnDelete(DeleteBehavior.Restrict);

        // relation with category
        builder.HasMany(a => a.Categories)
            .WithOne(c => c.Admin)
            .HasForeignKey(c => c.AdminId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
