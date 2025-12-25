using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Data.Context;

public interface ITaskManagementToolDatabase : IAsyncDisposable
{
    public DbContext DbContext { get; }

    public DatabaseFacade Database { get; }

    public DbSet<ToDoEntity> Todos { get; set; }

    public Task SaveChangesAsync();
}
