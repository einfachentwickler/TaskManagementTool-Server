using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.Contracts;

public interface ITodoRepository
{
    public Task<IEnumerable<TodoEntry>> GetAsync(int pageSize, int pageNumber);

    public Task<IEnumerable<TodoEntry>> GetAsync(string userId, int pageSize, int pageNumber);

    public Task<TodoEntry> FirstOrDefaultAsync(int id);

    public Task<TodoEntry> CreateAsync(TodoEntry item);

    public Task UpdateAsync(TodoEntry item);

    public Task DeleteAsync(Expression<Func<TodoEntry, bool>> predicate);
}