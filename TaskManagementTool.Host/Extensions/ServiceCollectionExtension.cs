using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.Host.Profiles;
using DbContext = TaskManagementTool.DataAccess.DbContext;

namespace TaskManagementTool.Host.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext>(options =>
                options.UseSqlServer(configuration.GetSection("ConnectionString").Value)
            );
        }
        public static void ConfigureCors(this IServiceCollection service)
            =>
                service.AddCors(
                    cors => cors.AddPolicy("CorsPolicy",
                        builder =>
                            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                    )
                );

        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 1;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddEntityFrameworkStores<DbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration.GetSection("AuthSettings:Audience").Value,
                    ValidIssuer = configuration["AuthSettings:Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetSection("AuthSettings:Key").Value)
                    ),
                    ValidateIssuerSigningKey = true
                };
            });
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            MapperConfiguration config = new(cf =>
                cf.AddProfile(new DefaultMappingProfile())
            );
            services.AddSingleton(config.CreateMapper());
        }
    }
}
