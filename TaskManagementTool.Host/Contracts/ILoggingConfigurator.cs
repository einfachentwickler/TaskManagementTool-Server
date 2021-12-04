using Microsoft.Extensions.Configuration;

namespace TaskManagementTool.Host.Contracts
{
    public interface ILoggingConfigurator
    {
        public void Setup(IConfiguration configuration);
    }
}
