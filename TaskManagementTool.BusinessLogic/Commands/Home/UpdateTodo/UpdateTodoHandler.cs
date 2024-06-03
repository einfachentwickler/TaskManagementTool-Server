using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.UpdateTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Home.UpdateTodo;

public class UpdateTodoHandler(
    ITodoRepository todoRepository,
    IAuthUtils authUtils,
    IValidator<UpdateTodoRequest> requestValidator,
    IMapper mapper
    ) : IRequestHandler<UpdateTodoRequest, UpdateTodoResponse>
{
    public async Task<UpdateTodoResponse> Handle(UpdateTodoRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        if (!await authUtils.IsAllowedAction(todoRepository, request.HttpContext, request.UpdateTodoDto.Id))
        {
            throw new TaskManagementToolException(ApiErrorCode.Forbidden, "");
        }

        TodoEntry item = await todoRepository.FirstOrDefaultAsync(request.UpdateTodoDto.Id);

        item.Name = request.UpdateTodoDto.Name;
        item.IsCompleted = request.UpdateTodoDto.IsCompleted;
        item.Content = request.UpdateTodoDto.Content;
        item.Importance = request.UpdateTodoDto.Importance;

        TodoEntry updateResult = await todoRepository.UpdateAsync(item);

        return new UpdateTodoResponse
        {
            Todo = mapper.Map<TodoDto>(updateResult)
        };
    }
}