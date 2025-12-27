using Application;
using Infrastructure.Context;
using Infrastructure.DI;
using Infrastructure.Seeding;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WebApi.Middleware;

namespace WebApi;

[ExcludeFromCodeCoverage]
public class Program
{
    public static async Task Main(string[] args)
    {
        //todo caching
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
        app.UseSwaggerUI();

        app.MapHealthChecks("/health");

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");

        app.UseRateLimiter();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHealthChecks()
            .AddCheck("health", () => HealthCheckResult.Healthy())
            .AddDbContextCheck<TaskManagementToolDbContext>(
                name: "sqlserver-db-check",
                tags: ["ready"],
                failureStatus: HealthStatus.Unhealthy,
                customTestQuery: async (context, cancellationToken) =>
                {
                    return await context.Database.CanConnectAsync(cancellationToken);
                });

        builder.Services
            .AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment())
            .AddApplication()
            .ConfigureHost(builder.Configuration);

        builder.ConfigureLogging(builder.Configuration);
    }
}