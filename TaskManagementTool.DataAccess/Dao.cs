using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess
{
    public class Dao : IdentityDbContext<User>
    {
        public DbSet<Todo> Todos { get; set; }

        public Dao(DbContextOptions<Dao> options) : base(options) { }
    }
}
