using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;

namespace TaskManagementTool.BusinessLogic.Interfaces;

public interface ITodoHandler
{
    public Task<IEnumerable<TodoDto>> GetAsync(int pageSize, int pageNumber);

    public Task<IEnumerable<TodoDto>> GetAsync(string userId, int pageSize, int pageNumber);

    public Task<TodoDto> FindByIdAsync(int id);

    public Task<TodoDto> CreateAsync(CreateTodoDto toDoPar);

    public Task<TodoDto> UpdateAsync(UpdateTodoDto todo);

    public Task DeleteAsync(int id);
}