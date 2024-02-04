using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementTool.BusinessLogic.Commands;
using TaskManagementTool.BusinessLogic.Commands.Auth.Register;
using TaskManagementTool.BusinessLogic.Commands.Auth.Register.Validation;
using TaskManagementTool.BusinessLogic.Commands.Utils.Jwt;
using TaskManagementTool.BusinessLogic.Commands.Wrappers;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.MappingProfiles;

namespace TaskManagementTool.BusinessLogic;

public static class DiModule
{
    public static void ConfigureBll(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserRegisterRequestValidator>();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<UserRegisterHandler>());

        services.AddAutoMapper(typeof(DefaultMappingProfile));

        services.AddScoped<IAdminHandler, AdminHandler>();

        services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();
        services.AddScoped<IJwtSecurityTokenBuilder, JwtSecurityTokenBuilder>();
    }
}