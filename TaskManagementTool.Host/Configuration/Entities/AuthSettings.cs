using Microsoft.Extensions.Configuration;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.Host.Configuration.Entities;

public class AuthSettings(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public string Audience => _configuration.GetSection("AuthSettings:Audience").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "AuthSettings:Audience");

    public string Issuer => _configuration.GetSection("AuthSettings:Issuer").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "AuthSettings:Issuer");

    public string Key => _configuration.GetSection("AuthSettings:Key").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "AuthSettings:Key");
}