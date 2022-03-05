using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        private readonly IMapper _mapper;

        public TodoService(IMapper mapper, ITodoRepository todoRepository) => (_todoRepository, _mapper) = (todoRepository, mapper);

        public async Task<IEnumerable<TodoDto>> GetAsync()
        {
            IEnumerable<Todo> todos = await _todoRepository.GetAsync();
            IEnumerable<TodoDto> mappedTodos = _mapper.Map<IEnumerable<TodoDto>>(todos);
            return mappedTodos;
        }

        public async Task<TodoDto> FirstAsync(int id)
        {
            Todo todo = await _todoRepository.GetSingleAsync(id);
            TodoDto mappedTodo = _mapper.Map<Todo, TodoDto>(todo);

            return mappedTodo;
        }

        public async Task AddAsync(CreateTodoDto todoPar)
        {
            Todo todo = _mapper.Map<CreateTodoDto, Todo>(todoPar);
            await _todoRepository.AddAsync(todo);
        }

        public async Task UpdateAsync(UpdateTodoDto todo)
        {
            Todo item = await _todoRepository.GetSingleAsync(todo.Id);

            item.Name = todo.Name;
            item.IsCompleted = todo.IsCompleted;
            item.Content = todo.Content;
            item.Importance = todo.Importance;

            await _todoRepository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id) => await _todoRepository.DeleteAsync(id);
    }
}
