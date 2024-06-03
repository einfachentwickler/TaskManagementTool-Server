using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.DatabaseContext;

public class TaskManagementToolDatabase : IdentityDbContext<UserEntry>, ITaskManagementToolDatabase
{
    public TaskManagementToolDatabase(DbContextOptions<TaskManagementToolDatabase> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<TodoEntry> Todos { get; set; }

    public DbContext DbContext => this;

    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}