
namespace MotoVerse.Infrastructure.Data.Configuration.Users;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Table name
        builder.ToTable("Customers");


    }
}
