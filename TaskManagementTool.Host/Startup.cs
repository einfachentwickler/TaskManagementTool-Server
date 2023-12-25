using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using TaskManagementTool.BusinessLogic;
using TaskManagementTool.Common.Configuration;
using TaskManagementTool.Host.Constants;
using TaskManagementTool.Host.Extensions;
using TaskManagementTool.Host.Middleware;

namespace TaskManagementTool.Host;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.ConfigureCors();

        services.ConfigureSqlContext(new DatabaseConfigurationOptions(Configuration));

        services.ConfigureIdentity(
            new IdentityConfigurationOptions(Configuration),
            new TokenValidationOptions(Configuration),
            new AuthSettings(Configuration)
            );

        services.RegisterDependencies();

        services.ConfigureBll();

        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        static void ConfigureSwagger(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint(SwaggerSetupConstants.URL, SwaggerSetupConstants.APPLICATION_NAME);
        }

        app.UseSwagger();

        app.UseSwaggerUI(ConfigureSwagger);

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();
        app.UseCors(CorsPolicyNameConstants.DEFAULT_POLICY_NAME);

        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(builder => builder.MapControllers());
    }
}
