using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskManagementTool.DataAccess;
using TaskManagementTool.Host;

namespace IntegrationTests;

public class TmtWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptionsBuilder<TaskManagementToolDatabase>));

            services.AddDbContext<TaskManagementToolDatabase>(
                options => options.UseSqlServer(GetConnectionString(),
                builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null))
                );

            TaskManagementToolDatabase db = CreateDbContext(services);

            db.Database.EnsureDeleted();
        });
    }

    private static string? GetConnectionString()
    {
        IConfigurationRoot builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory + "/Configuration")
            .AddJsonFile("appsettings.test.json")
            .Build();

        return builder.GetSection("ConnectionString").Value;
    }

    private static TaskManagementToolDatabase CreateDbContext(IServiceCollection services)
    {
        ServiceProvider provider = services.BuildServiceProvider();
        IServiceScope scope = provider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<TaskManagementToolDatabase>();
    }
}