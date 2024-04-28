using Microsoft.Extensions.Configuration;

namespace TaskManagementTool.Common.Configuration.Startup;

public class DatabaseConfigurationOptions(IConfiguration configuration)
{
	public string DatabaseName => configuration.GetSection("DockerDatabaseConfiguration:DBName").Value;

	public string Server => configuration.GetSection("DockerDatabaseConfiguration:DBServer").Value;

	public string Port => configuration.GetSection("DockerDatabaseConfiguration:DBPort").Value;

	public string User => configuration.GetSection("DockerDatabaseConfiguration:DBUser").Value;

	public string Password => configuration.GetSection("DockerDatabaseConfiguration:DBPassword").Value;
}