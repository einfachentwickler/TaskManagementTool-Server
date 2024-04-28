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
using TaskManagementTool.Common.Configuration.Startup;
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

		services.ConfigureIdentity(
			new IdentityConfigurationOptions(configuration),
			new TokenValidationOptions(configuration),
			configuration
		);

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

	private static void ConfigureIdentity(
		this IServiceCollection services,
		IdentityConfigurationOptions passwordOptions,
		TokenValidationOptions tokenOptions,
		IConfiguration configuration
		)
	{
		void AddIdentity(IdentityOptions identityOptions)
		{
			identityOptions.Password.RequireDigit = passwordOptions.IsDigitRequired;
			identityOptions.Password.RequireLowercase = passwordOptions.IsLowercaseRequired;
			identityOptions.Password.RequireNonAlphanumeric = passwordOptions.IsNonAlphaNumericRequired;
			identityOptions.Password.RequireUppercase = passwordOptions.IsUppercaseRequired;
			identityOptions.Password.RequiredLength = passwordOptions.RequiredPasswordLength;
			identityOptions.Password.RequiredUniqueChars = passwordOptions.RequiredUniqueChart;

			identityOptions.User.RequireUniqueEmail = true;
		}

		static void AddAuthentication(AuthenticationOptions options)
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}

		void AddJwtBearer(JwtBearerOptions options)
		{
			//TODO get sensitive info from env variables
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = tokenOptions.ShouldValidateIssuer,
				ValidateAudience = tokenOptions.ShouldValidateAudience,
				ValidAudience = configuration["AuthSettings:Audience"],
				ValidIssuer = configuration["AuthSettings:Issuer"],
				RequireExpirationTime = tokenOptions.ShouldRequireExpirationTime,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AuthSettings:Key"]!)),
				ValidateIssuerSigningKey = tokenOptions.ShouldValidateIssuerSigninKey
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