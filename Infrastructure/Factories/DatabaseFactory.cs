using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Factories;

public class DatabaseFactory(DbContextOptions<TaskManagementToolDbContext> options) : IDatabaseFactory
{
    public ITaskManagementToolDatabase Create()
    {
        return new TaskManagementToolDbContext(options);
    }
}