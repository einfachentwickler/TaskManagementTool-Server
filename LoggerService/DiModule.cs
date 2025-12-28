using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Web;
using Shared.Configuration;
using LogLevel = NLog.LogLevel;

namespace LoggerService;

public static class DiModule
{
    public static void ConfigureLogging(this WebApplicationBuilder builder, IConfiguration configuration, bool isDevelopmentEnv)
    {
        builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        LoggingConfiguration config = new();

        DatabaseTarget dbTarget = new()
        {
            ConnectionString = BuildConnectionString(configuration, isDevelopmentEnv),
            CommandText = "INSERT INTO [ct_logs] (created_on,message,level,exception,stack_trace,logger,url) VALUES (@datetime,@msg,@level,@exception,@trace,@logger,@url);"
        };

        dbTarget.Parameters.Add(new DatabaseParameterInfo("@datetime", new SimpleLayout("${date}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@msg", new SimpleLayout("${message}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", new SimpleLayout("${level}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", new SimpleLayout("${exception}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@trace", new SimpleLayout("${stacktrace}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@logger", new SimpleLayout("${logger}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@url", new SimpleLayout("${aspnet-request-url}")));

        config.AddTarget("database", dbTarget);

        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, dbTarget));
        config.LoggingRules.Add(new LoggingRule("LoggerService.LoggerManager", LogLevel.Trace, dbTarget));

        LogManager.Configuration = config;
    }

    private static string BuildConnectionString(IConfiguration configuration, bool isDevEnv)
    {
        if (isDevEnv)
        {
            return configuration.GetRequiredSection("DevConnectionStrings:SqlServerDataBaseLoggerConnectionString").Value!;
        }

        var dockerConfig = configuration.GetRequiredSection(nameof(DockerDatabaseOptions)).Get<DockerDatabaseOptions>()!;

        return $"Server={dockerConfig.DbNameLogs},{dockerConfig.DBPort};Initial Catalog={dockerConfig.DBName};User ID={dockerConfig.DBUser};Password={dockerConfig.DBPassword};TrustServerCertificate=True";
    }
}