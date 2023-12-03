using Microsoft.Extensions.Configuration;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.Common.Configuration;

public class DatabaseConfigurationOptions(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public string ConnectionString => _configuration.GetSection("ConnectionString").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "ConnectionString");
}