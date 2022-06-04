﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.Contracts
{
    public interface ITodoRepository
    {
        public Task<IEnumerable<TodoEntry>> GetAsync(SearchCriteriaEnum searchCriteria, string userId = null);

        public Task<TodoEntry> FirstAsync(int id);

        public Task AddAsync(TodoEntry item);

        public Task UpdateAsync(TodoEntry item);

        public Task DeleteAsync(int id);
    }
}
