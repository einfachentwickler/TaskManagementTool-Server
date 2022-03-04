using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.DataAccess.Contracts
{
    public interface ITodoRepository
    {
        public Task<IEnumerable<Todo>> GetAsync();

        public Task<Todo> GetSingleAsync(int id);

        public Task AddAsync(Todo item);

        public Task UpdateAsync(Todo item);

        public Task DeleteAsync(int id);
    }
}
