using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.Services;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Repositories;
using TaskManagementTool.Host.Configuration.Entities;
using TaskManagementTool.Host.Extensions;
using TaskManagementTool.Host.Middleware;

namespace TaskManagementTool.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(_ => new IdentityConfigurationOptions(Configuration));
            services.AddTransient(_ => new DatabaseConfigurationOptions(Configuration));
            services.AddTransient(_ => new TokenValidationOptions(Configuration));
            services.AddTransient(_ => new AuthSettings(Configuration));

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
