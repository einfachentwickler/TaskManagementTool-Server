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

namespace TaskManagementTool.BusinessLogic.Services;

public class TodoHandler(IMapper mapper, ITodoRepository todoRepository) : ITodoHandler
{
    private readonly ITodoRepository _todoRepository = todoRepository;

    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<TodoDto>> GetAsync(SearchCriteriaEnum searchCriteria, string userId = null)
    {
        IEnumerable<TodoEntry> todos = await _todoRepository.GetAsync(searchCriteria, userId);

        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    public async Task<TodoDto> FindByIdAsync(int id)
    {
        TodoEntry todoEntry = await _todoRepository.FirstOrDefaultAsync(id);

        return todoEntry is null ? throw new TaskManagementToolException(ApiErrorCode.TodoNotFound, $"Todo with id {id} was not found") : _mapper.Map<TodoEntry, TodoDto>(todoEntry);
    }

    public async Task<TodoDto> CreateAsync(CreateTodoDto todoPar)
    {
        TodoEntry todoEntry = _mapper.Map<CreateTodoDto, TodoEntry>(todoPar);

        return _mapper.Map<TodoEntry, TodoDto>(await _todoRepository.CreateAsync(todoEntry));
    }

    public async Task UpdateAsync(UpdateTodoDto todo)
    {
        TodoEntry item = await _todoRepository.FirstOrDefaultAsync(todo.Id);

        item.Name = todo.Name;
        item.IsCompleted = todo.IsCompleted;
        item.Content = todo.Content;
        item.Importance = todo.Importance;

        await _todoRepository.UpdateAsync(item);
    }

    public async Task DeleteAsync(int id)
    {
        _ = await _todoRepository.FirstOrDefaultAsync(id) ?? throw new TaskManagementToolException(ApiErrorCode.TodoNotFound, $"Todo with id {id} was not found");

        await _todoRepository.DeleteAsync(id);
    }
}