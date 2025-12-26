using Infrastructure.Entities;
using Infrastructure.EntityConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class TaskManagementToolDbContext(DbContextOptions<TaskManagementToolDbContext> options) : IdentityDbContext<UserEntity>(options), ITaskManagementToolDbContext
{
    public DbSet<ToDoEntity> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new UserEntityConfiguration());
        builder.ApplyConfiguration(new ToDoEntityConfiguration());
    }
}