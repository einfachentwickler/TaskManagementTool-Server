using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.Host.Contracts;

namespace TaskManagementTool.Host.Logging
{
    public class LoggingConfigurator : ILoggingConfigurator
    {
        private static LogLevel ParseToLogLevel(string level)
        {
            return level switch
            {
                "Information" => LogLevel.Info,
                "Error" => LogLevel.Error,
                _ => throw new TaskManagementToolException("Invalid log level settings")
            };
        }

        public void Setup(IConfiguration configuration)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            DatabaseTarget dbTarget = new DatabaseTarget
            {
                ConnectionString = configuration.GetSection("Logging:Configuration:ConnectionString").Value
                                   ?? throw new TaskManagementToolException("Connection string to logging database is null"),

                CommandText = "insert into Logs(message, log_level, datetime) values(@message,@level, @time_stamp);"
            };

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@time_stamp", new NLog.Layouts.SimpleLayout("${date}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", new NLog.Layouts.SimpleLayout("${level}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@message", new NLog.Layouts.SimpleLayout("${message}")));

            config.AddTarget("database", dbTarget);

            LogLevel microsoftLogLevel = ParseToLogLevel(configuration.GetSection("Logging:LogLevel:MicrosoftLogLevel").Value);
            LogLevel metricsServiceLogLevel = ParseToLogLevel(configuration.GetSection("Logging:LogLevel:TaskManagementTool").Value);

            config.LoggingRules.Add(new LoggingRule("Microsoft.*", microsoftLogLevel, dbTarget));
            config.AddRuleForAllLevels(new NullTarget(), "Microsoft.*", true);
            config.LoggingRules.Add(new LoggingRule("*", metricsServiceLogLevel, dbTarget));

            LogManager.Configuration = config;
            InternalLogger.LogFile = @"C:\Users\d8vel\Desktop\VS_Pr\TaskManagementTool\Server";
        }
    }
}
