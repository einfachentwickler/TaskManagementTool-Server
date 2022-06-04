using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess
{
    public class TaskManagementToolDatabase : IdentityDbContext<User>, ITaskManagementToolDatabase
    {
        public TaskManagementToolDatabase(DbContextOptions<TaskManagementToolDatabase> options) : base(options) { }

        public DbSet<TodoEntry> Todos { get; set; }

        public DbContext DbContext => this;

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
