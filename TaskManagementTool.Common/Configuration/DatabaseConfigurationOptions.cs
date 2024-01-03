using Microsoft.Extensions.Configuration;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.Common.Configuration;

public class DatabaseConfigurationOptions(IConfiguration configuration)
{
    public string ConnectionString => configuration.GetSection("ConnectionString").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "ConnectionString");

    public string DatabaseName => configuration["DBName"] ?? "TaskManagementTool";

    public string Server => configuration["DBServer"] ?? "ms-sql-server";

    public string Port => configuration["DBPort"] ?? "1433";

#warning refactor
    public string User => configuration["DBUser"] ?? "SA";

#warning refactor
    public string Password => configuration["DBPassword"] ?? "Password123455";
}