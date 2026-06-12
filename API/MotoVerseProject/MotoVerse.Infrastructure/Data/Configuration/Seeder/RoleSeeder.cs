using MotoVerse.Entities.Models.Auth;

namespace MotoVerse.Infrastructure.Data.Configuration.Seeder;

public class RoleConfiguration : IEntityTypeConfiguration<MyRole>
{
    public void Configure(EntityTypeBuilder<MyRole> builder)
    {
        builder.HasData(
            new MyRole
            {
                Id = "F1413B91-312B-42DF-916B-BA615D04CBCD",
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            },
            new MyRole
            {
                Id = "F1413B91-312B-42DF-916B-BA615D04CBCF",
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
    }
}