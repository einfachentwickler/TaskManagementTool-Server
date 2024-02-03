using Microsoft.Extensions.Configuration;

namespace TaskManagementTool.Common.Configuration.Startup;

public class AuthSettings(IConfiguration configuration)
{
    public string Audience => configuration.GetSection("AuthSettings:Audience").Value;

    public string Issuer => configuration.GetSection("AuthSettings:Issuer").Value;

    public string Key => configuration.GetSection("AuthSettings:Key").Value;
}