using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.Host.Constants;
using TaskManagementTool.Host.Profiles;
using DbContext = TaskManagementTool.DataAccess.DbContext;

namespace TaskManagementTool.Host.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetSection(DbConfigurationSectionNames.CONNECTION_STRING).Value;

            void UseSqlServer(DbContextOptionsBuilder builder) => builder.UseSqlServer(connectionString);

            services.AddDbContext<DbContext>(UseSqlServer);
        }

        public static void ConfigureCors(this IServiceCollection service)
        {
            void AddPolicy(CorsPolicyBuilder builder) => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();

            void AddCors(CorsOptions options) => options.AddPolicy(CorsPolicyNames.DEFAULT_POLICY_NAME, AddPolicy);

            service.AddCors(AddCors);
        }

        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            void AddIdentity(IdentityOptions options)
            {
                options.Password.RequireDigit = bool.Parse(configuration.GetSection(PasswordOptionsSectionNames.REQUIRE_DIGITS).Value);
                options.Password.RequireLowercase = bool.Parse(configuration.GetSection(PasswordOptionsSectionNames.REQUIRE_LOWERCASE).Value);
                options.Password.RequireNonAlphanumeric = bool.Parse(configuration.GetSection(PasswordOptionsSectionNames.REQUIRE_NON_ALPHANUMERIC).Value);
                options.Password.RequireUppercase = bool.Parse(configuration.GetSection(PasswordOptionsSectionNames.REQUIRE_UPPERCASE).Value);
                options.Password.RequiredLength = int.Parse(configuration.GetSection(PasswordOptionsSectionNames.REQUIRED_LENGTH).Value);
                options.Password.RequiredUniqueChars = int.Parse(configuration.GetSection(PasswordOptionsSectionNames.REQUIRE_UNIQUE_CHARTS).Value);
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
                    ValidateIssuer = bool.Parse(configuration.GetSection(TokenValidationParameterSectionNames.SHOULD_VALIDATE_ISSUER).Value),
                    ValidateAudience = bool.Parse(configuration.GetSection(TokenValidationParameterSectionNames.SHOULD_VALIDATE_AUDIENCE).Value),
                    ValidAudience = configuration.GetSection(AuthSettingsSectionNames.AUDIENCE).Value,
                    ValidIssuer = configuration[AuthSettingsSectionNames.ISSUER],
                    RequireExpirationTime = bool.Parse(configuration.GetSection(TokenValidationParameterSectionNames.SHOULD_REQUIRE_EXPIRATION_TIME).Value),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetSection(AuthSettingsSectionNames.KEY).Value)
                    ),
                    ValidateIssuerSigningKey = bool.Parse(configuration.GetSection(TokenValidationParameterSectionNames.SHOULD_VALIDATE_ISSUER_SIGNIN_KEY).Value)
                };
            }

            services
                .AddIdentity<User, IdentityRole>(AddIdentity)
                .AddEntityFrameworkStores<DbContext>()
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
