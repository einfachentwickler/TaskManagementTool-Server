using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess;

public class TaskManagementToolDatabase(DbContextOptions<TaskManagementToolDatabase> options) : IdentityDbContext<User>(options), ITaskManagementToolDatabase
{
    public DbSet<TodoEntry> Todos { get; set; }

    public DbContext DbContext => this;

    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}