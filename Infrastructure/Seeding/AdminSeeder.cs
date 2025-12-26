using Infrastructure.ConfigurationModels;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Constants;
using System.Threading.Tasks;

namespace Infrastructure.Seeding;

public static class AdminSeeder
{
    public static async Task SeedAsync(UserManager<UserEntity> userManager, DefaultAdminCredentials adminConfig)
    {
        if (await userManager.FindByEmailAsync(adminConfig.Email) != null) 
            return;

        var admin = new UserEntity
        {
            Email = adminConfig.Email,
            UserName = adminConfig.Email,
            FirstName = adminConfig.FirstName,
            LastName = adminConfig.LastName,
            Age = adminConfig.Age,
            IsBlocked = false,
            Role = adminConfig.Role
        };

        var result = await userManager.CreateAsync(admin, adminConfig.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, UserRoles.ADMIN_ROLE);
        }
    }
}