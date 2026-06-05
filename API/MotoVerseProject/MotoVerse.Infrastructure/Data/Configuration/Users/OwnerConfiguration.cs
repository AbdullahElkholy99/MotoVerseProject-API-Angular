
namespace MotoVerse.Infrastructure.Data.Configuration.Users;

internal class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        // Table name
        builder.ToTable("Owners");


    }
}
