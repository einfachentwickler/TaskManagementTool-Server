using Infrastructure.Data.Context;
using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TaskManagementTool.Common.Constants;

namespace Infrastructure.Initializers;

public static class EfCoreCodeFirstInitializer
{
    public static async Task InitializeAsync(
        ITaskManagementToolDbContext context,
        UserManager<UserEntity> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration
        )
    {
        UserEntity admin = new()
        {
            Age = int.Parse(configuration.GetSection("AdminCredentials:Age").Value),
            Email = configuration.GetSection("AdminCredentials:Email").Value,
            IsBlocked = bool.Parse(configuration.GetSection("AdminCredentials:IsBlocked").Value),
            UserName = configuration.GetSection("AdminCredentials:Email").Value,
            Role = configuration.GetSection("AdminCredentials:Role").Value,
            FirstName = configuration.GetSection("AdminCredentials:FirstName").Value,
            LastName = configuration.GetSection("AdminCredentials:LastName").Value,
        };

        if (await roleManager.FindByNameAsync(UserRoles.ADMIN_ROLE) is null)
        {
            IdentityRole adminRole = new(UserRoles.ADMIN_ROLE);

            await roleManager.CreateAsync(adminRole);
        }

        if (await roleManager.FindByNameAsync(UserRoles.USER_ROLE) is null)
        {
            IdentityRole userRole = new(UserRoles.USER_ROLE);

            await roleManager.CreateAsync(userRole);
        }

        if (await userManager.FindByNameAsync(admin.Email) is null)
        {
            IdentityResult result = await userManager.CreateAsync(admin, configuration.GetSection("AdminCredentials:Password").Value);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, UserRoles.ADMIN_ROLE);
            }

            ToDoEntity[] todos =
            [
                new()
                {
                    Name = "Todo 1 init",
                    Content = "Todo 1 content",
                    IsCompleted = false,
                    Importance = 10,
                    Creator = admin
                },
                new()
                {
                    Name = "Todo 2 init",
                    Content = "Todo 2 content",
                    IsCompleted = true,
                    Importance = 2,
                    Creator = admin
                }
            ];

            foreach (ToDoEntity todo in todos)
            {
                await context.Todos.AddAsync(todo);
            }

            await context.SaveChangesAsync(default);
        }
    }
}