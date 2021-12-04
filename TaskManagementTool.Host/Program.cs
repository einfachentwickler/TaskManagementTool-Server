using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using NLog.Web;
using TaskManagementTool.DataAccess;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Initializers;

namespace TaskManagementTool.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                UserManager<User> userManager = services.GetRequiredService<UserManager<User>>();
                RoleManager<IdentityRole> rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                DbContext context = services.GetRequiredService<DbContext>();

                if (await context.Database.EnsureCreatedAsync())
                {
                    await DbInitializer.InitializeAsync(context, userManager, rolesManager);
                }
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
