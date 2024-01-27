using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic;
using TaskManagementTool.Common.Configuration;
using TaskManagementTool.DataAccess;
using TaskManagementTool.DataAccess.DatabaseContext;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Initializers;
using TaskManagementTool.Host.Constants;
using TaskManagementTool.Host.Middleware;

namespace TaskManagementTool.Host;

[ExcludeFromCodeCoverage]
[SuppressMessage("Roslynator", "RCS1102:Make class static", Justification = "Used in integration tests")]
public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.ConfigureCors();

        builder.Services.ConfigureDataAccess(new DatabaseConfigurationOptions(builder.Configuration));

        builder.Services.ConfigureIdentity(
            new IdentityConfigurationOptions(builder.Configuration),
            new TokenValidationOptions(builder.Configuration),
            new AuthSettings(builder.Configuration)
            );

        builder.Services.RegisterDependencies();

        builder.Services.ConfigureBll();

        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        using IServiceScope scope = app.Services.CreateScope();

        app.UseSwagger();

        app.UseSwaggerUI(options => options.SwaggerEndpoint(SwaggerSetupConstants.URL, SwaggerSetupConstants.APPLICATION_NAME));

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();
        app.UseCors(CorsPolicyNameConstants.DEFAULT_POLICY_NAME);

        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();

        UserManager<User> userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        RoleManager<IdentityRole> rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        TaskManagementToolDatabase context = scope.ServiceProvider.GetRequiredService<TaskManagementToolDatabase>();

        if (!await context.Database.EnsureCreatedAsync())
        {
            await EfCoreCodeFirstInitializer.InitializeAsync(context, userManager, rolesManager, builder.Configuration);
        }

        await app.RunAsync();
    }
}