using Microsoft.Extensions.Configuration;

namespace TaskManagementTool.Common.Configuration;

public class TokenValidationOptions(IConfiguration configuration)
{
    public bool ShouldValidateIssuer => bool.Parse(configuration.GetSection("TokenValidationParameters:ValidateIssuer").Value);

    public bool ShouldValidateAudience => bool.Parse(configuration.GetSection("TokenValidationParameters:ValidateAudience").Value);

    public bool ShouldValidateIssuerSigninKey => bool.Parse(configuration.GetSection("TokenValidationParameters:ValidateIssuerSigningKey").Value);

    public bool ShouldRequireExpirationTime => bool.Parse(configuration.GetSection("TokenValidationParameters:RequireExpirationTime").Value);
}