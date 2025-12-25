using Application.Commands.Auth.Register;
using Application.Commands.Auth.Register.Validation;
using Application.Commands.Utils.Jwt;
using Application.Commands.Wrappers;
using Application.MappingProfiles;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

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