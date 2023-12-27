using Microsoft.Extensions.Configuration;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.Common.Configuration;

public class AuthSettings(IConfiguration configuration)
{
    public string Audience => configuration.GetSection("AuthSettings:Audience").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "AuthSettings:Audience");

    public string Issuer => configuration.GetSection("AuthSettings:Issuer").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "AuthSettings:Issuer");

    public string Key => configuration.GetSection("AuthSettings:Key").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "AuthSettings:Key");
}