using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.Common.Configuration;
using TaskManagementTool.DataAccess.DatabaseContext;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.Host.Constants;

namespace TaskManagementTool.Host;

[ExcludeFromCodeCoverage]
public static class DiModule
{
    public static IServiceCollection ConfigureHost(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthUtils, AuthUtils>();

        services
            .AddOptions<AuthSettings>()
            .BindConfiguration(nameof(AuthSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.ConfigureIdentity(configuration);

        services.ConfigureCors();

        services.AddSwaggerGen();

        return services;
    }

    private static void ConfigureCors(this IServiceCollection service)
    {
        void AddPolicy(CorsPolicyBuilder builder) => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();

        void AddCors(CorsOptions options) => options.AddPolicy(CorsPolicyNameConstants.DEFAULT_POLICY_NAME, AddPolicy);

        service.AddCors(AddCors);
    }

    private static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        //TODO refresh token
        //TODO get sensitive info from env variables
        IConfigurationSection identitySection = configuration.GetRequiredSection("IdentityPasswordOptions");

        IConfigurationSection tokenValidationSection = configuration.GetRequiredSection("TokenValidationParameters");

        IConfigurationSection authSection = configuration.GetRequiredSection("AuthSettings");

        void AddIdentity(IdentityOptions identityOptions)
        {
            identityOptions.Password.RequireDigit = bool.Parse(identitySection["RequireDigit"]!);
            identityOptions.Password.RequireLowercase = bool.Parse(identitySection["RequireLowercase"]!);
            identityOptions.Password.RequireNonAlphanumeric = bool.Parse(identitySection["RequireNonAlphanumeric"]!);
            identityOptions.Password.RequireUppercase = bool.Parse(identitySection["RequireUppercase"]!);
            identityOptions.Password.RequiredLength = int.Parse(identitySection["RequiredLength"]!);
            identityOptions.Password.RequiredUniqueChars = int.Parse(identitySection["RequiredUniqueChars"]!);

            identityOptions.User.RequireUniqueEmail = true;
        }

        static void AddAuthentication(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        void AddJwtBearer(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = bool.Parse(tokenValidationSection["ValidateIssuer"]!),
                ValidateAudience = bool.Parse(tokenValidationSection["ValidateAudience"]!),
                ValidAudience = authSection["Audience"],
                ValidIssuer = authSection["Issuer"],
                RequireExpirationTime = bool.Parse(tokenValidationSection["RequireExpirationTime"]!),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSection["Key"]!)),
                ValidateIssuerSigningKey = bool.Parse(tokenValidationSection["ValidateIssuerSigningKey"]!)
            };
        }

        services
            .AddIdentity<User, IdentityRole>(AddIdentity)
            .AddEntityFrameworkStores<TaskManagementToolDatabase>()
            .AddDefaultTokenProviders();

        services
            .AddAuthentication(AddAuthentication)
            .AddJwtBearer(AddJwtBearer);
    }
}