using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess
{
    public class DbContext : IdentityDbContext<User>
    {
        public DbSet<Todo> Todos { get; set; }

        public DbContext(DbContextOptions<DbContext> options) : base(options) { }
    }
}
