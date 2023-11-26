using Microsoft.Extensions.DependencyInjection;
using TaskManagementTool.BusinessLogic.Contracts;
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
        services.AddTransient<ITodoRepository, TodoRepository>();
        services.AddTransient<ITodoService, TodoService>();

        services.AddTransient<IAdminService, AdminService>();
        services.AddTransient<IAuthService, AuthService>();

        services.AddTransient<IDatabaseFactory, DatabaseFactory>();

        services.AddTransient<IAuthUtils, AuthUtils>();
    }
}