using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        private readonly IMapper _mapper;

        public TodoService(IMapper mapper, ITodoRepository todoRepository) => (_todoRepository, _mapper) = (todoRepository, mapper);

        public async Task<IEnumerable<TodoDto>> GetAsync(SearchCriteriaEnum searchCriteria, string userId = null)
        {
            IEnumerable<TodoEntry> todos = await _todoRepository.GetAsync(searchCriteria, userId);

            IEnumerable<TodoDto> mappedTodos = _mapper.Map<IEnumerable<TodoDto>>(todos);
            return mappedTodos;
        }

        public async Task<TodoDto> FindByIdAsync(int id)
        {
            TodoEntry todoEntry = await _todoRepository.FirstAsync(id);
            TodoDto mappedTodo = _mapper.Map<TodoEntry, TodoDto>(todoEntry);

            return mappedTodo;
        }

        public async Task AddAsync(CreateTodoDto todoPar)
        {
            TodoEntry todoEntry = _mapper.Map<CreateTodoDto, TodoEntry>(todoPar);
            await _todoRepository.AddAsync(todoEntry);
        }

        public async Task UpdateAsync(UpdateTodoDto todo)
        {
            TodoEntry item = await _todoRepository.FirstAsync(todo.Id);

            item.Name = todo.Name;
            item.IsCompleted = todo.IsCompleted;
            item.Content = todo.Content;
            item.Importance = todo.Importance;

            await _todoRepository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id) => await _todoRepository.DeleteAsync(id);
    }
}
