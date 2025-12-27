using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Context;

public interface ITaskManagementToolDbContext
{
    DbSet<ToDoEntity> Todos { get; }
    DbSet<UserEntity> Users { get; }
    DbSet<RefreshTokenEntity> RefreshTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}