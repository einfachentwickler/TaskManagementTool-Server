using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.Initializers
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            DbContext context, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            const string password = "password";

            if (await roleManager.FindByNameAsync("Admin") is null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await userManager.FindByNameAsync(DataSeed.Users.First().Email) is null)
            {
                IdentityResult result = await userManager.CreateAsync(DataSeed.Users.First(), password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(DataSeed.Users.First(), "Admin");
                }

                List<Todo> collection = DataSeed.Todos.ToList();
                collection.ForEach(async td => await context.Todos.AddAsync(td));
                await context.SaveChangesAsync();
            }
        }
    }
}
