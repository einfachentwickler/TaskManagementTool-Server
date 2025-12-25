using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.Context;

public interface ITaskManagementToolDbContext : IAsyncDisposable
{
    public DbContext DbContext { get; }

    public DatabaseFacade Database { get; }

    public DbSet<ToDoEntity> Todos { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}