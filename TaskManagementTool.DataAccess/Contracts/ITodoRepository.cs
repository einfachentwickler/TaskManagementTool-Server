using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.Contracts;

public interface ITodoRepository
{
    public Task<IEnumerable<TodoEntry>> GetAsync(SearchCriteriaEnum searchCriteria, string userId = null);

    public Task<TodoEntry> FirstOrDefaultAsync(int id);

    public Task<TodoEntry> CreateAsync(TodoEntry item);

    public Task UpdateAsync(TodoEntry item);

    public Task DeleteAsync(Expression<Func<TodoEntry, bool>> predicate);
}