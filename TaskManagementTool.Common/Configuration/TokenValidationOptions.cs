using Microsoft.Extensions.Configuration;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.Common.Configuration;

public class TokenValidationOptions(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public bool ShouldValidateIssuer => bool.Parse(_configuration.GetSection("TokenValidationParameters:ValidateIssuer").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "TokenValidationParameters:ValidateIssuer"));

    public bool ShouldValidateAudience => bool.Parse(_configuration.GetSection("TokenValidationParameters:ValidateAudience").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "TokenValidationParameters:ValidateAudience"));

    public bool ShouldValidateIssuerSigninKey => bool.Parse(_configuration.GetSection("TokenValidationParameters:ValidateIssuerSigningKey").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "TokenValidationParameters:ValidateIssuerSigningKey"));

    public bool ShouldRequireExpirationTime => bool.Parse(_configuration.GetSection("TokenValidationParameters:RequireExpirationTime").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "TokenValidationParameters:ShouldRequireExpirationTime"));
}