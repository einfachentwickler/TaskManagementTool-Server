using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodoById.Models;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Home.GetTodoById;
public class GetTodoByIdHandler(
    IAuthUtils authUtils,
    ITodoRepository todoRepository,
    IValidator<GetTodoByIdRequest> requestValidator,
    IMapper mapper
    ) : IRequestHandler<GetTodoByIdRequest, GetTodoByIdResponse>
{
    public async Task<GetTodoByIdResponse> Handle(GetTodoByIdRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        if (!await authUtils.IsAllowedAction(todoRepository, request.HttpContext, request.TodoId))
        {
            throw new TaskManagementToolException(ApiErrorCode.Forbidden, "");
        }

        TodoEntry todoEntry = await todoRepository.FirstOrDefaultAsync(request.TodoId);

        TodoDto result = todoEntry is null
            ? throw new TaskManagementToolException(ApiErrorCode.TodoNotFound, $"Todo with id {request.TodoId} was not found")
            : mapper.Map<TodoEntry, TodoDto>(todoEntry);

        return new GetTodoByIdResponse { Todo = result };
    }
}