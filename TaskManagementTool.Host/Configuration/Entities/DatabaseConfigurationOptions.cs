using Microsoft.Extensions.Configuration;

namespace TaskManagementTool.Host.Configuration.Entities
{
    public class DatabaseConfigurationOptions
    {
        private readonly IConfiguration _configuration;

        public DatabaseConfigurationOptions(IConfiguration configuration) => _configuration = configuration;

        public string ConnectionString => _configuration.GetSection("ConnectionString").Value;
    }
}
