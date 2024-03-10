using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
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
            services.RemoveAll(typeof(DbContextOptions<TaskManagementToolDatabase>));

            //ToDo move to docker, add test db initialization script
            services.AddDbContext<TaskManagementToolDatabase>(
                options => options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TaskManagementToolTests;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null))
                );

            TaskManagementToolDatabase db = CreateDbContext(services);

            db.Database.EnsureDeleted();
        });
    }

    private static TaskManagementToolDatabase CreateDbContext(IServiceCollection services)
    {
        ServiceProvider provider = services.BuildServiceProvider();
        IServiceScope scope = provider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<TaskManagementToolDatabase>();
    }
}