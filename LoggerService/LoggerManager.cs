using NLog;

namespace LoggerService;

public class LoggerManager : ILoggerManager
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    public void LogDebug(string message) => logger.Debug(message);
    public void LogError(Exception exception) => logger.Error(exception);
    public void LogInfo(string message) => logger.Info(message);
    public void LogWarn(string message) => logger.Warn(message);
}