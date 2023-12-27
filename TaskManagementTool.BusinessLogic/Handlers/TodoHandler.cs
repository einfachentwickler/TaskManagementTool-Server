using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Handlers;

public class TodoHandler(IMapper mapper, ITodoRepository todoRepository) : ITodoHandler
{
    public async Task<IEnumerable<TodoDto>> GetAsync(int pageSize, int pageNumber)
    {
        IEnumerable<TodoEntry> todos = await todoRepository.GetAsync(pageSize, pageNumber);

        return mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    public async Task<IEnumerable<TodoDto>> GetAsync(string userId, int pageSize, int pageNumber)
    {
        IEnumerable<TodoEntry> todos = await todoRepository.GetAsync(userId, pageSize, pageNumber);

        return mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    public async Task<TodoDto> FindByIdAsync(int id)
    {
        TodoEntry todoEntry = await todoRepository.FirstOrDefaultAsync(id);

        return todoEntry is null ? throw new TaskManagementToolException(ApiErrorCode.TodoNotFound, $"Todo with id {id} was not found") : mapper.Map<TodoEntry, TodoDto>(todoEntry);
    }

    public async Task<TodoDto> CreateAsync(CreateTodoDto todoPar)
    {
        TodoEntry todoEntry = mapper.Map<CreateTodoDto, TodoEntry>(todoPar);

        return mapper.Map<TodoEntry, TodoDto>(await todoRepository.CreateAsync(todoEntry));
    }

    public async Task UpdateAsync(UpdateTodoDto todo)
    {
        TodoEntry item = await todoRepository.FirstOrDefaultAsync(todo.Id);

        item.Name = todo.Name;
        item.IsCompleted = todo.IsCompleted;
        item.Content = todo.Content;
        item.Importance = todo.Importance;

        await todoRepository.UpdateAsync(item);
    }

    public async Task DeleteAsync(int id)
    {
        _ = await todoRepository.FirstOrDefaultAsync(id) ?? throw new TaskManagementToolException(ApiErrorCode.TodoNotFound, $"Todo with id {id} was not found");

        await todoRepository.DeleteAsync(x => x.Id == id);
    }
}