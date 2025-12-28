using Application.Commands.Auth.Register;
using Application.Commands.Auth.Register.Validation;
using Application.Services.Http;
using Application.Services.IdentityUserManagement;
using Application.Services.Jwt.AccessToken;
using Application.Services.Jwt.RefreshToken;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DiModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserRegisterCommandValidator>();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<UserRegisterHandler>());

        services.AddScoped<IIdentityUserManagerWrapper, IdentityUserManagerWrapper>();
        services.AddScoped<IJwtAccessTokenBuilder, JwtAccessTokenBuilder>();
        services.AddScoped<IHttpContextDataExtractor, HttpContextDataExtractor>();
        services.AddSingleton<IJwtRefreshTokenGenerator, JwtRefreshTokenGenerator>();

        return services;
    }
}