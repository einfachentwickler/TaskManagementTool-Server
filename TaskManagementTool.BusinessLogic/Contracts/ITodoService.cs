using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;

namespace TaskManagementTool.BusinessLogic.Contracts
{
    public interface ITodoService
    {
        public Task<IEnumerable<TodoDto>> GetAsync();

        public Task<TodoDto> GetSingleAsync(int id);

        public Task AddAsync(CreateTodoDto toDoPar);

        public Task UpdateAsync(UpdateTodoDto todo);

        public Task DeleteAsync(int id);
    }
}
