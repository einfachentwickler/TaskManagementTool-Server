using Infrastructure.Contracts;
using Infrastructure.Data.Context;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DiModule
{
    public static IServiceCollection ConfigureDataAccess(this IServiceCollection services, IConfiguration configuration, bool isDevMode)
    {
        string connectionString = BuildConnectionString(configuration, isDevMode);

        services.AddDbContext<TaskManagementToolDbContext>(builder => builder.UseSqlServer(connectionString));

        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<IDatabaseFactory, DatabaseFactory>();

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