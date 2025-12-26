using Microsoft.AspNetCore.Identity;
using Shared.Constants;
using System.Threading.Tasks;

namespace Infrastructure.Seeding;

public static class RolesSeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { UserRoles.ADMIN_ROLE, UserRoles.USER_ROLE };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}