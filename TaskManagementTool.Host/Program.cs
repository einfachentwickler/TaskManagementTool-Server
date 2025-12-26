using Application;
using Infrastructure.Context;
using Infrastructure.DI;
using Infrastructure.Seeding;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TaskManagementTool.Host.Constants;
using TaskManagementTool.Host.Middleware;

namespace TaskManagementTool.Host;

[ExcludeFromCodeCoverage]
public class Program
{
    public static async Task Main(string[] args)
    {
        //todo health check, rate limiting, caching
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

        ConfigureServices(builder);

        WebApplication app = builder.Build();

        ConfigureMiddleware(app);

        await ApplyDatabaseMigrationAsync(app);

        await app.RunAsync();
    }

    private static async Task ApplyDatabaseMigrationAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        var dbContext = services.GetRequiredService<TaskManagementToolDbContext>();

        await dbContext.Database.MigrateAsync();

        await DbInitializer.SeedAsync(services);
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options => options.SwaggerEndpoint(SwaggerSetupConstants.URL, SwaggerSetupConstants.APPLICATION_NAME));

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();
        app.UseCors(CorsPolicyNameConstants.DEFAULT_POLICY_NAME);

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        builder.Services
            .AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment())
            .AddApplication()
            .ConfigureHost(builder.Configuration);

        builder.ConfigureLogging(builder.Configuration);
    }
}