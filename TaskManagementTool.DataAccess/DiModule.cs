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
    public static void ConfigureDataAccess(this IServiceCollection services, DatabaseConfigurationOptions options)
    {
        string connectionString = BuildConnectionString(options);

        services.AddDbContext<TaskManagementToolDatabase>(builder => builder.UseSqlServer(connectionString));

        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<IDatabaseFactory, DatabaseFactory>();
    }

    private static string BuildConnectionString(DatabaseConfigurationOptions options)
    {
        return $"Server={options.Server},{options.Port};Initial Catalog={options.DatabaseName};User ID={options.User};Password={options.Password};TrustServerCertificate=True";
    }
}