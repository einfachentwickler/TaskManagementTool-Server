using Microsoft.EntityFrameworkCore;

namespace TaskManagementTool.DataAccess.Factories
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly DbContextOptions<TaskManagementToolDatabase> _options;

        public DatabaseFactory(DbContextOptions<TaskManagementToolDatabase> options) => _options = options;

        public ITaskManagementToolDatabase Create()
        {
            return new TaskManagementToolDatabase(_options);
        }
    }
}
