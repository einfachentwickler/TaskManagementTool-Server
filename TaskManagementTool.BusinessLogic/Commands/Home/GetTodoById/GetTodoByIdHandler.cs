using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Contracts;
using Infrastructure.Data.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodoById.Models;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.BusinessLogic.Dto;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

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

        var todoEntry = await todoRepository.FirstOrDefaultAsync(request.TodoId);

        TodoDto result = todoEntry is null
            ? throw new TaskManagementToolException(ApiErrorCode.TodoNotFound, $"Todo with id {request.TodoId} was not found")
            : mapper.Map<ToDoEntity, TodoDto>(todoEntry);

        return new GetTodoByIdResponse { Todo = result };
    }
}