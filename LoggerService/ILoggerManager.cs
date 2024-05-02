namespace LoggerService;

public interface ILoggerManager
{
    public void LogDebug(string message);
    public void LogError(Exception exception);
    public void LogInfo(string message);
    public void LogWarn(string message);
}