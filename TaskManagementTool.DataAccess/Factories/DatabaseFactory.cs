using Microsoft.EntityFrameworkCore;
using TaskManagementTool.DataAccess.DatabaseContext;

namespace TaskManagementTool.DataAccess.Factories;

public class DatabaseFactory(DbContextOptions<TaskManagementToolDatabase> options) : IDatabaseFactory
{
    public ITaskManagementToolDatabase Create()
    {
        return new TaskManagementToolDatabase(options);
    }
}