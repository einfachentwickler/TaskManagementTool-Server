using Microsoft.Extensions.Configuration;

namespace TaskManagementTool.Common.Configuration.Startup;

public class LocalEnvSettings(IConfiguration configuration)
{
	public string SqlServerDataBaseConnectionString => configuration.GetSection("LocalSettings:SqlServerDataBaseConnectionString").Value;
	public string SqlServerLoggerConnectionString => configuration.GetSection("LocalSettings:SqlServerLoggerConnectionString").Value;
}