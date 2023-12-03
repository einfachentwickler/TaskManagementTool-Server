using Microsoft.Extensions.Configuration;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.Common.Configuration;

public class IdentityConfigurationOptions(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public int RequiredPasswordLength => int.Parse(_configuration.GetSection("IdentityPasswordOptions:RequiredLength").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequiredLength"));

    public int RequiredUniqueChart => int.Parse(_configuration.GetSection("IdentityPasswordOptions:RequiredUniqueChars").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequiredUniqueChars"));

    public bool IsDigitRequired => bool.Parse(_configuration.GetSection("IdentityPasswordOptions:RequireDigit").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequireDigit"));

    public bool IsLowercaseRequired => bool.Parse(_configuration.GetSection("IdentityPasswordOptions:RequireLowercase").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequireLowercase"));

    public bool IsNonAlphaNumericRequired => bool.Parse(_configuration.GetSection("IdentityPasswordOptions:RequireNonAlphanumeric").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequireNonAlphanumeric"));

    public bool IsUppercaseRequired => bool.Parse(_configuration.GetSection("IdentityPasswordOptions:RequireUppercase").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequireUppercase"));
}