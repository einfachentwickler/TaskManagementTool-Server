using Microsoft.Extensions.Configuration;

namespace TaskManagementTool.Host.Configuration.Entities
{
    public class TokenValidationOptions
    {
        private readonly IConfiguration _configuration;

        public TokenValidationOptions(IConfiguration configuration) => _configuration = configuration;

        public bool ShouldValidateIssuer => bool.Parse(_configuration.GetSection("TokenValidationParameters:ValidateIssuer").Value);

        public bool ShouldValidateAudience => bool.Parse(_configuration.GetSection("TokenValidationParameters:ValidateAudience").Value);

        public bool ShouldValidateIssuerSigninKey => bool.Parse(_configuration.GetSection("TokenValidationParameters:ValidateIssuerSigningKey").Value);

        public bool ShouldRequireExpirationTime => bool.Parse(_configuration.GetSection("TokenValidationParameters:RequireExpirationTime").Value);
    }
}
