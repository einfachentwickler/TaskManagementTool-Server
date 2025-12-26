using Application.Configuration;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TaskManagementTool.Host.Constants;

namespace TaskManagementTool.Host;

[ExcludeFromCodeCoverage]
public static class DiModule
{
    public static IServiceCollection ConfigureHost(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<AuthSettings>()
            .BindConfiguration(nameof(AuthSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.ConfigureIdentity(configuration);

        services.ConfigureCors();

        return services;
    }

    private static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyNameConstants.DEFAULT_POLICY_NAME, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });
    }

    private static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        //todo add emails
        //adjust swagger, produceresponsetype
        //add login as
        //TODO refresh token
        //TODO get sensitive info from env variables
        var identityOptions = configuration.GetRequiredSection(nameof(IdentityPasswordOptions)).Get<IdentityPasswordOptions>()!;
        var authOptions = configuration.GetRequiredSection(nameof(AuthSettings)).Get<AuthSettings>()!;
        var tokenValidationOptions = configuration.GetRequiredSection(nameof(TokenValidationOptions)).Get<TokenValidationOptions>()!;

        services.AddIdentity<UserEntity, IdentityRole>(options =>
        {
            options.Password.RequireDigit = identityOptions.RequireDigit;
            options.Password.RequireLowercase = identityOptions.RequireLowercase;
            options.Password.RequireNonAlphanumeric = identityOptions.RequireNonAlphanumeric;
            options.Password.RequireUppercase = identityOptions.RequireUppercase;
            options.Password.RequiredLength = identityOptions.RequiredLength;
            options.Password.RequiredUniqueChars = identityOptions.RequiredUniqueChars;

            options.User.RequireUniqueEmail = true;
        })
       .AddEntityFrameworkStores<TaskManagementToolDbContext>()
       .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = tokenValidationOptions.ValidateIssuer,
                ValidateAudience = tokenValidationOptions.ValidateAudience,
                ValidIssuer = authOptions.Issuer,
                ValidAudience = authOptions.Audience,
                RequireExpirationTime = tokenValidationOptions.RequireExpirationTime,
                ValidateIssuerSigningKey = tokenValidationOptions.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Key)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}