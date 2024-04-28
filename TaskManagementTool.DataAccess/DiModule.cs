using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementTool.Common.Configuration.Startup;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.DatabaseContext;
using TaskManagementTool.DataAccess.Factories;
using TaskManagementTool.DataAccess.Repositories;

namespace TaskManagementTool.DataAccess;
public static class DiModule
{
    public static IServiceCollection ConfigureDataAccess(
        this IServiceCollection services,
        DatabaseConfigurationOptions options,
        LocalEnvSettings localEnvSettings,
        bool isDevMode
        )
    {
        string connectionString = BuildConnectionString(options, localEnvSettings, isDevMode);

        services.AddDbContext<TaskManagementToolDatabase>(builder => builder.UseSqlServer(connectionString));

        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<IDatabaseFactory, DatabaseFactory>();

        return services;
    }

    private static string BuildConnectionString(DatabaseConfigurationOptions options, LocalEnvSettings localEnvSettings, bool isDevMode)
    {
        if (isDevMode)
        {
            return localEnvSettings.SqlServerDataBaseConnectionString;
        }

        return $"Server={options.Server},{options.Port};Initial Catalog={options.DatabaseName};User ID={options.User};Password={options.Password};TrustServerCertificate=True";
    }
}