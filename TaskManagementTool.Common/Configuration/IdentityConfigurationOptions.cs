using Microsoft.Extensions.Configuration;

namespace TaskManagementTool.Common.Configuration;

public class IdentityConfigurationOptions(IConfiguration configuration)
{
    public int RequiredPasswordLength => int.Parse(configuration.GetSection("IdentityPasswordOptions:RequiredLength").Value);

    public int RequiredUniqueChart => int.Parse(configuration.GetSection("IdentityPasswordOptions:RequiredUniqueChars").Value);

    public bool IsDigitRequired => bool.Parse(configuration.GetSection("IdentityPasswordOptions:RequireDigit").Value);

    public bool IsLowercaseRequired => bool.Parse(configuration.GetSection("IdentityPasswordOptions:RequireLowercase").Value);

    public bool IsNonAlphaNumericRequired => bool.Parse(configuration.GetSection("IdentityPasswordOptions:RequireNonAlphanumeric").Value);

    public bool IsUppercaseRequired => bool.Parse(configuration.GetSection("IdentityPasswordOptions:RequireUppercase").Value);
}