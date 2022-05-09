using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.BusinessLogic.Services.Utils;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Repositories;
using TaskManagementTool.Host.Configuration.Constants;
using TaskManagementTool.Host.Configuration.Entities;
using TaskManagementTool.Host.Extensions;
using TaskManagementTool.Host.Middleware;

namespace TaskManagementTool.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.ConfigureAutoMapper();

            services.ConfigureCors();

            services.ConfigureSqlContext(new DatabaseConfigurationOptions(Configuration));

            services.ConfigureIdentity(
                new IdentityConfigurationOptions(Configuration),
                new TokenValidationOptions(Configuration),
                new AuthSettings(Configuration)
                );

            services.AddTransient<ITodoRepository, TodoRepository>();
            services.AddTransient<ITodoService, TodoService>();

            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IAuthUtils, AuthUtils>();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            static void ConfigureSwagger(SwaggerUIOptions options)
            {
                options.SwaggerEndpoint(SwaggerSetupConstants.URL, SwaggerSetupConstants.APPLICATION_NAME);
            }

            static void ConfigureEndpoints(IEndpointRouteBuilder builder) => builder.MapControllers();

            app.UseSwagger();

            app.UseSwaggerUI(ConfigureSwagger);

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors(CorsPolicyNameConstants.DEFAULT_POLICY_NAME);

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(ConfigureEndpoints);
        }
    }
}
