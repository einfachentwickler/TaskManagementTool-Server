using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Configuration;
using Shared.Constants;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.RateLimiting;

namespace WebApi;

[ExcludeFromCodeCoverage]
public static class DiModule
{
    public static IServiceCollection ConfigureHost(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<AuthOptions>()
            .BindConfiguration(nameof(AuthOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.ConfigureIdentity(configuration);

        services.ConfigureRateLimiter(configuration);

        services.ConfigureCors();

        return services;
    }

    private static IServiceCollection ConfigureRateLimiter(this IServiceCollection services, IConfiguration configuration)
    {
        var concurrencyRateLimiterOptions = configuration.GetRequiredSection(nameof(ConcurrencyRateLimiterOptions)).Get<ConcurrencyRateLimiterOptions>()!;

        services.AddRateLimiter(options => options.AddConcurrencyLimiter(policyName: RateLimiterConstants.CONCURRENCY_POLICY_NAME, options =>
            {
                options.PermitLimit = concurrencyRateLimiterOptions.PermitLimit;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = concurrencyRateLimiterOptions.QueueLimit;
            }));
        return services;
    }

    private static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            //TODO setup cors after UI setup
            options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });
    }

    private static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        //adjust swagger, produceresponsetype
        //add login as
        //TODO get sensitive info from env variables
        //todo hateoas
        var identityOptions = configuration.GetRequiredSection(nameof(IdentityPasswordOptions)).Get<IdentityPasswordOptions>()!;
        var authOptions = configuration.GetRequiredSection(nameof(AuthOptions)).Get<AuthOptions>()!;
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