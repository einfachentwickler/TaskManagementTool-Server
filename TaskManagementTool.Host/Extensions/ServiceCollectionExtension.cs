using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagementTool.DataAccess;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.Host.Configuration.Constants;
using TaskManagementTool.Host.Configuration.Entities;
using TaskManagementTool.Host.Configuration.Profiles;

namespace TaskManagementTool.Host.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureSqlContext(this IServiceCollection services, DatabaseConfigurationOptions options)
        {
            void UseSqlServer(DbContextOptionsBuilder builder) => builder.UseSqlServer(options.ConnectionString);

            services.AddDbContext<TaskManagementToolDatabase>(UseSqlServer);
        }

        public static void ConfigureCors(this IServiceCollection service)
        {
            void AddPolicy(CorsPolicyBuilder builder) => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();

            void AddCors(CorsOptions options) => options.AddPolicy(CorsPolicyNameConstants.DEFAULT_POLICY_NAME, AddPolicy);

            service.AddCors(AddCors);
        }

        public static void ConfigureIdentity(
            this IServiceCollection services,
            IdentityConfigurationOptions passwordOptions,
            TokenValidationOptions tokenOptions,
            AuthSettings authSettings
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
                    ValidateIssuer = tokenOptions.ShouldValidateIssuer,
                    ValidateAudience = tokenOptions.ShouldValidateAudience,
                    ValidAudience = authSettings.Audience,
                    ValidIssuer = authSettings.Issuer,
                    RequireExpirationTime = tokenOptions.ShouldRequireExpirationTime,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.Key)),
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

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            DefaultMappingProfile defaultProfile = new();

            void AddProfile(IMapperConfigurationExpression expression) => expression.AddProfile(defaultProfile);

            MapperConfiguration config = new(AddProfile);

            IMapper mapper = config.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
