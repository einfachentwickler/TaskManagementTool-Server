using Infrastructure;
using Infrastructure.Data.Context;
using Infrastructure.Data.Entities;
using Infrastructure.Initializers;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic;
using TaskManagementTool.Host.Constants;
using TaskManagementTool.Host.Middleware;

namespace TaskManagementTool.Host;

[ExcludeFromCodeCoverage]
public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

        builder.Services.AddControllers();

        builder.Services
            .AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment())
            .ConfigureBll()
            .ConfigureHost(builder.Configuration);

        builder.ConfigureLogging(builder.Configuration);

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

        UserManager<UserEntity> userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
        RoleManager<IdentityRole> rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        ITaskManagementToolDbContext context = scope.ServiceProvider.GetRequiredService<TaskManagementToolDbContext>();

        if (!await context.Database.EnsureCreatedAsync())
        {
            await EfCoreCodeFirstInitializer.InitializeAsync(context, userManager, rolesManager, builder.Configuration);
        }

        await app.RunAsync();
    }
}