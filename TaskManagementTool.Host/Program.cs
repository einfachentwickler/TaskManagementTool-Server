using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using System;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Initializers;

namespace TaskManagementTool.Host
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            using IServiceScope scope = host.Services.CreateScope();

            IServiceProvider services = scope.ServiceProvider;
            IConfiguration configuration = services.GetRequiredService<IConfiguration>();

            UserManager<User> userManager = services.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            TaskManagementToolDatabase context = services.GetRequiredService<TaskManagementToolDatabase>();

            if (!await context.Database.EnsureCreatedAsync())
            {
                await EfCoreCodeFirstInitializer.InitializeAsync(context, userManager, rolesManager, configuration);
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder(args)
                .UseNLog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>();
                });
    }
}
