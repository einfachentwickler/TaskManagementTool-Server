using Microsoft.EntityFrameworkCore;
using TaskManagementTool.DataAccess.DatabaseContext;

namespace TaskManagementTool.DataAccess.Factories;

public class DatabaseFactory(DbContextOptions<TaskManagementToolDatabase> options) : IDatabaseFactory
{
    private readonly DbContextOptions<TaskManagementToolDatabase> _options = options;

    public ITaskManagementToolDatabase Create()
    {
        return new TaskManagementToolDatabase(_options);
    }
}