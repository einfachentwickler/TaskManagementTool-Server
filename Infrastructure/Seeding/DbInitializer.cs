using Infrastructure.ConfigurationModels;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Seeding;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<ITaskManagementToolDbContext>();
        var userManager = services.GetRequiredService<UserManager<UserEntity>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var config = services.GetRequiredService<IConfiguration>();
        var adminConfig = config.GetSection("AdminCredentials").Get<DefaultAdminCredentials>();

        await RolesSeeder.SeedAsync(roleManager);
        await AdminSeeder.SeedAsync(userManager, adminConfig);
        await TodosSeeder.SeedAsync(context, adminConfig.Email);
    }
}