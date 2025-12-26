using Application.Commands.Auth.Register;
using Application.Commands.Auth.Register.Validation;
using Application.MappingProfiles;
using Application.Services.IdentityUserManagement;
using Application.Services.Jwt;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DiModule
{
    public static IServiceCollection ConfigureBll(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserRegisterCommandValidator>();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<UserRegisterHandler>());

        services.AddAutoMapper(x => { }, typeof(DefaultMappingProfile));

        services.AddScoped<IIdentityUserManagerWrapper, IdentityUserManagerWrapper>();
        services.AddScoped<IJwtSecurityTokenBuilder, JwtSecurityTokenBuilder>();

        return services;
    }
}