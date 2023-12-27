using Microsoft.Extensions.Configuration;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.Common.Configuration;

public class IdentityConfigurationOptions(IConfiguration configuration)
{
    public int RequiredPasswordLength => int.Parse(configuration.GetSection("IdentityPasswordOptions:RequiredLength").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequiredLength"));

    public int RequiredUniqueChart => int.Parse(configuration.GetSection("IdentityPasswordOptions:RequiredUniqueChars").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequiredUniqueChars"));

    public bool IsDigitRequired => bool.Parse(configuration.GetSection("IdentityPasswordOptions:RequireDigit").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequireDigit"));

    public bool IsLowercaseRequired => bool.Parse(configuration.GetSection("IdentityPasswordOptions:RequireLowercase").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequireLowercase"));

    public bool IsNonAlphaNumericRequired => bool.Parse(configuration.GetSection("IdentityPasswordOptions:RequireNonAlphanumeric").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequireNonAlphanumeric"));

    public bool IsUppercaseRequired => bool.Parse(configuration.GetSection("IdentityPasswordOptions:RequireUppercase").Value ?? throw new TaskManagementToolException(ConfigErrorMessagesConstants.CONFIG_VALUE_NOT_FOUND + "IdentityPasswordOptions:RequireUppercase"));
}