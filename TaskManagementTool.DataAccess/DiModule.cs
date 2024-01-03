using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementTool.Common.Configuration;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.DatabaseContext;
using TaskManagementTool.DataAccess.Factories;
using TaskManagementTool.DataAccess.Repositories;

namespace TaskManagementTool.DataAccess;
public static class DiModule
{
    public static void ConfigureDataAccess(this IServiceCollection services, DatabaseConfigurationOptions options)
    {
#warning refactor
        string connectionString = $"Server={options.Server},{options.Port};Initial Catalog={options.DatabaseName};User ID={options.User};Password={options.Password};TrustServerCertificate=True";

        void UseSqlServer(DbContextOptionsBuilder builder) => builder.UseSqlServer(connectionString);

        services.AddDbContext<TaskManagementToolDatabase>(UseSqlServer);

        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<IDatabaseFactory, DatabaseFactory>();
    }
}