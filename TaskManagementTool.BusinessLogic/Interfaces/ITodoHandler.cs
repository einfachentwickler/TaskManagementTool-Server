using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Interfaces;

public interface ITodoHandler
{
    public Task<IEnumerable<TodoDto>> GetAsync(SearchCriteriaEnum searchCriteria, string userId = null);

    public Task<TodoDto> FindByIdAsync(int id);

    public Task AddAsync(CreateTodoDto toDoPar);

    public Task UpdateAsync(UpdateTodoDto todo);

    public Task DeleteAsync(int id);
}