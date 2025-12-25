using Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Contracts;

public interface ITodoRepository
{
    public Task<IEnumerable<ToDoEntity>> GetAsync(int pageSize, int pageNumber);

    public Task<IEnumerable<ToDoEntity>> GetAsync(string userId, int pageSize, int pageNumber);

    public Task<ToDoEntity> FirstOrDefaultAsync(int id);

    public Task<ToDoEntity> CreateAsync(ToDoEntity item);

    public Task<ToDoEntity> UpdateAsync(ToDoEntity item);

    public Task DeleteAsync(Expression<Func<ToDoEntity, bool>> predicate);
}