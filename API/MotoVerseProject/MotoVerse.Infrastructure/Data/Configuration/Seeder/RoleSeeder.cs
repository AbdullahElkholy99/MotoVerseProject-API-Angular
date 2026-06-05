using Microsoft.AspNetCore.Identity;


namespace MotoVerse.Infrastructure.Data.Configuration.Seeder;

public static class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<MotoVerse.Data.Entities.Auth.Role> _roleManager)
    {
        var rolesCount = await _roleManager.Roles.CountAsync();
        if (rolesCount <= 0)
        {

            await _roleManager.CreateAsync(new MotoVerse.Data.Entities.Auth.Role()
            {
                Name = "Admin"
            });
            await _roleManager.CreateAsync(new MotoVerse.Data.Entities.Auth.Role()
            {
                Name = "User"
            });
            await _roleManager.CreateAsync(new MotoVerse.Data.Entities.Auth.Role()
            {
                Name = "Owner"
            });
            await _roleManager.CreateAsync(new MotoVerse.Data.Entities.Auth.Role()
            {
                Name = "Customer"
            });
        }
    }

}
