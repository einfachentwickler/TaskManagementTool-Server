using Microsoft.Extensions.Configuration;

namespace TaskManagementTool.Host.Configuration.Entities
{
    public class AuthSettings
    {
        private readonly IConfiguration _configuration;

        public AuthSettings(IConfiguration configuration) => _configuration = configuration;

        public string Audience => _configuration.GetSection("AuthSettings:Audience").Value;

        public string Issuer => _configuration.GetSection("AuthSettings:Issuer").Value;

        public string Key => _configuration.GetSection("AuthSettings:Key").Value;
    }
}
