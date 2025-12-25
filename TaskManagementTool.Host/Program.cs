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
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

        builder.Services.AddControllers();

        builder.Services
            .AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment())
            .ConfigureBll()
            .ConfigureHost(builder.Configuration);

        builder.ConfigureLogging(builder.Configuration);

        WebApplication app = builder.Build();

        app.UseSwagger();

        app.UseSwaggerUI(options => options.SwaggerEndpoint(SwaggerSetupConstants.URL, SwaggerSetupConstants.APPLICATION_NAME));

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();
        app.UseCors(CorsPolicyNameConstants.DEFAULT_POLICY_NAME);

        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<TaskManagementToolDbContext>();

            await context.Database.MigrateAsync();

            await DbInitializer.SeedAsync(services);
        }

        await app.RunAsync();
    }
}