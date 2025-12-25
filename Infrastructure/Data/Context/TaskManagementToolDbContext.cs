using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Data.Context;

public class TaskManagementToolDbContext : IdentityDbContext<UserEntity>, ITaskManagementToolDatabase
{
    public TaskManagementToolDbContext(DbContextOptions<TaskManagementToolDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<ToDoEntity> Todos { get; set; }

    public DbContext DbContext => this;

    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}