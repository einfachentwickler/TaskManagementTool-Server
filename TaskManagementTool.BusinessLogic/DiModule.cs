using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementTool.BusinessLogic.Handlers;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.MappingProfiles;
using TaskManagementTool.BusinessLogic.Validation;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;

namespace TaskManagementTool.BusinessLogic;

public static class DiModule
{
    public static void ConfigureBll(this IServiceCollection services)
    {
#warning TODO - fix auto register option
        services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();

        services.AddAutoMapper(typeof(DefaultMappingProfile));

        services.AddScoped<IAdminHandler, AdminHandler>();
        services.AddScoped<IAuthHandler, AuthHandler>();
    }
}