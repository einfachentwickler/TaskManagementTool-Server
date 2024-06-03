using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.DatabaseContext;

public interface ITaskManagementToolDatabase : IAsyncDisposable
{
    public DbContext DbContext { get; }

    public DatabaseFacade Database { get; }

    public DbSet<TodoEntry> Todos { get; set; }

    public Task SaveChangesAsync();
}
