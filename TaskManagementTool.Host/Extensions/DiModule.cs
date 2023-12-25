using Microsoft.Extensions.DependencyInjection;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.BusinessLogic.Services.Utils;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Factories;
using TaskManagementTool.DataAccess.Repositories;

namespace TaskManagementTool.Host.Extensions;

public static class DiModule
{
    public static void RegisterDependencies(this IServiceCollection services)
    {
        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<ITodoHandler, TodoHandler>();

        services.AddScoped<IDatabaseFactory, DatabaseFactory>();

        services.AddScoped<IAuthUtils, AuthUtils>();
    }
}