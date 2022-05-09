using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.Contracts
{
    public interface ITodoRepository
    {
        public Task<IEnumerable<Todo>> GetAsync(SearchCriteriaEnum searchCriteria, string userId = null);

        public Task<Todo> FirstAsync(int id);

        public Task AddAsync(Todo item);

        public Task UpdateAsync(Todo item);

        public Task DeleteAsync(int id);
    }
}
