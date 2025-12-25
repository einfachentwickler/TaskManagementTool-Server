using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class DiModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevMode)
    {
        string connectionString = BuildConnectionString(configuration, isDevMode);

        services.AddDbContext<TaskManagementToolDbContext>(builder => builder.UseSqlServer(connectionString));

        services.AddScoped<ITaskManagementToolDbContext, TaskManagementToolDbContext>();

        return services;
    }

    private static string BuildConnectionString(IConfiguration configuration, bool isDevMode)
    {
        if (isDevMode)
        {
            return configuration.GetRequiredSection("LocalSettings:SqlServerDataBaseConnectionString").Value;
        }

        IConfigurationSection section = configuration.GetRequiredSection("DockerDatabaseConfiguration");

        return $"Server={section["DBServer"]},{section["DBPort"]};Initial Catalog={section["DBName"]};User ID={section["DBUser"]};Password={section["DBPassword"]};TrustServerCertificate=True";
    }
}