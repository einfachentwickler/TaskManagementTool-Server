using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.CreateTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Home.CreateTodo;

public class CreateTodoHandler(
    IAuthUtils authUtils,
    ITodoRepository todoRepository,
    IMapper mapper,
    IValidator<CreateTodoRequest> requestValidator
    ) : IRequestHandler<CreateTodoRequest, CreateTodoResponse>
{
    public async Task<CreateTodoResponse> Handle(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        request.CreateTodoDto.CreatorId = authUtils.GetUserId(request.HttpContext);

        TodoEntry todoEntry = mapper.Map<CreateTodoDto, TodoEntry>(request.CreateTodoDto);

        return new CreateTodoResponse { Todo = mapper.Map<TodoEntry, TodoDto>(await todoRepository.CreateAsync(todoEntry)) };
    }
}