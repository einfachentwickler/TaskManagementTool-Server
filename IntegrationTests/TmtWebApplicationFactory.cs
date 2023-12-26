using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskManagementTool.DataAccess.DatabaseContext;
using TaskManagementTool.Host;

namespace IntegrationTests;

public class TmtWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptionsBuilder<TaskManagementToolDatabase>));

            string? connString = GetConnectionString(services);

            services.AddDbContext<TaskManagementToolDatabase>(
                options => options.UseSqlServer(connString,
                builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null))
                );

            TaskManagementToolDatabase db = CreateDbContext(services);

            db.Database.EnsureDeleted();
        });
    }

    private static string? GetConnectionString(IServiceCollection services)
    {
        ServiceProvider provider = services.BuildServiceProvider();

        IConfiguration configuration = provider.GetRequiredService<IConfiguration>();

        return configuration.GetSection("TestConnectionString").Value;
    }

    private static TaskManagementToolDatabase CreateDbContext(IServiceCollection services)
    {
        ServiceProvider provider = services.BuildServiceProvider();
        IServiceScope scope = provider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<TaskManagementToolDatabase>();
    }
}