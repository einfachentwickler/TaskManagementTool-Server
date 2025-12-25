using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementTool.BusinessLogic.Commands.Auth.Register;
using TaskManagementTool.BusinessLogic.Commands.Auth.Register.Validation;
using TaskManagementTool.BusinessLogic.Commands.Utils.Jwt;
using TaskManagementTool.BusinessLogic.Commands.Wrappers;
using TaskManagementTool.BusinessLogic.MappingProfiles;

namespace TaskManagementTool.BusinessLogic;

public static class DiModule
{
    public static IServiceCollection ConfigureBll(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserRegisterRequestValidator>();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<UserRegisterHandler>());

        services.AddAutoMapper(x => { }, typeof(DefaultMappingProfile));

        services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();
        services.AddScoped<IJwtSecurityTokenBuilder, JwtSecurityTokenBuilder>();

        return services;
    }
}