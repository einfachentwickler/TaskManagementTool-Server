using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configuration;
using System;

namespace Infrastructure.DI;

public static class DiModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevEnv)
    {
        string connectionString = BuildConnectionString(configuration, isDevEnv);

        services.AddDbContext<TaskManagementToolDbContext>(builder => builder.UseSqlServer(connectionString, options =>
        {
            options.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        }));

        services.AddScoped<ITaskManagementToolDbContext, TaskManagementToolDbContext>();

        return services;
    }

    private static string BuildConnectionString(IConfiguration configuration, bool isDevEnv)
    {
        if (isDevEnv)
        {
            return configuration.GetRequiredSection("DevConnectionStrings:SqlServerDataBaseConnectionString").Value;
        }

        var dockerConfig = configuration.GetRequiredSection(nameof(DockerDatabaseOptions)).Get<DockerDatabaseOptions>();

        return $"Server={dockerConfig.DBServer},{dockerConfig.DBPort};Initial Catalog={dockerConfig.DBName};User ID={dockerConfig.DBUser};Password={dockerConfig.DBPassword};TrustServerCertificate=True";
    }
}