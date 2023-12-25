using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.Initializers
{
    public static class EfCoreCodeFirstInitializer
    {
        public static async Task InitializeAsync(
            ITaskManagementToolDatabase context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration
            )
        {
#warning TODO move to config
            const string password = "password";

            User admin = new()
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
                IdentityResult result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, UserRoles.ADMIN_ROLE);
                }

                TodoEntry[] todos =
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

                foreach (TodoEntry todo in todos)
                {
                    await context.Todos.AddAsync(todo);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
