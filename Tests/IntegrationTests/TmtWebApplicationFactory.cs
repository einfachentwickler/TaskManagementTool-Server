using Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApi;

namespace IntegrationTests;

public class TmtWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<TaskManagementToolDbContext>>();

            //ToDo move to docker for CI purposes
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var connectionString = configuration.GetSection("ConnectionStrings:LocalDbConnection").Value;

            services.AddDbContext<TaskManagementToolDbContext>(
                options => options.UseSqlServer(connectionString,
                builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null))
                );

            var db = CreateDbContext(services);

            db.Database.EnsureDeleted();
        });
    }

    private static TaskManagementToolDbContext CreateDbContext(IServiceCollection services)
    {
        ServiceProvider provider = services.BuildServiceProvider();
        IServiceScope scope = provider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<TaskManagementToolDbContext>();
    }
}