using Microsoft.Extensions.Configuration;

namespace TaskManagementTool.Host.Configuration.Entities
{
    public class IdentityConfigurationOptions
    {
        private readonly IConfiguration _configuration;

        public IdentityConfigurationOptions(IConfiguration configuration) => _configuration = configuration;

        public int RequiredPasswordLength => int.Parse(_configuration.GetSection("IdentityPasswordOptions:RequiredLength").Value);

        public int RequiredUniqueChart => int.Parse(_configuration.GetSection("IdentityPasswordOptions:RequiredUniqueChars").Value);
        
        public bool IsDigitRequired => bool.Parse(_configuration.GetSection("IdentityPasswordOptions:RequireDigit").Value);

        public bool IsLowercaseRequired => bool.Parse(_configuration.GetSection("IdentityPasswordOptions:RequireLowercase").Value);

        public bool IsNonAlphaNumericRequired => bool.Parse(_configuration.GetSection("IdentityPasswordOptions:RequireNonAlphanumeric").Value);

        public bool IsUppercaseRequired => bool.Parse(_configuration.GetSection("IdentityPasswordOptions:RequireUppercase").Value);
    }
}
