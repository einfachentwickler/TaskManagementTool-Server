using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Context;

public class TaskManagementToolDbContext : IdentityDbContext<UserEntity>, ITaskManagementToolDbContext
{
    public TaskManagementToolDbContext(DbContextOptions<TaskManagementToolDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<ToDoEntity> Todos { get; set; }

    public DbContext DbContext => this;
}