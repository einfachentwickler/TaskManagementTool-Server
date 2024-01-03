using Microsoft.Extensions.Configuration;

namespace TaskManagementTool.Common.Configuration;

public class DatabaseConfigurationOptions(IConfiguration configuration)
{
    public string DatabaseName => configuration.GetSection("DatabaseConfiguration:DBName").Value;

    public string Server => configuration.GetSection("DatabaseConfiguration:DBServer").Value;

    public string Port => configuration.GetSection("DatabaseConfiguration:DBPort").Value;

    public string User => configuration.GetSection("DatabaseConfiguration:DBUser").Value;

    public string Password => configuration.GetSection("DatabaseConfiguration:DBPassword").Value;
}