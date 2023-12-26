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
        void UseSqlServer(DbContextOptionsBuilder builder) => builder.UseSqlServer(options.ConnectionString);

        services.AddDbContext<TaskManagementToolDatabase>(UseSqlServer);

        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<IDatabaseFactory, DatabaseFactory>();
    }
}