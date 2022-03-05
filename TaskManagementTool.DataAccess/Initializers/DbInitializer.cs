using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.Initializers
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            Dao context, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            const string password = "password";

            if (await roleManager.FindByNameAsync(UserRoles.ADMIN_ROLE) is null)
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN_ROLE));
            }

            if (await userManager.FindByNameAsync(DataSeed.Users.First().Email) is null)
            {
                IdentityResult result = await userManager.CreateAsync(DataSeed.Users.First(), password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(DataSeed.Users.First(), UserRoles.ADMIN_ROLE);
                }

                List<Todo> collection = DataSeed.Todos.ToList();

                foreach (Todo todo in collection)
                {
                    await context.Todos.AddAsync(todo);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
