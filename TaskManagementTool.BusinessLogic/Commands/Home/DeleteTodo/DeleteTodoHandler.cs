using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.DeleteTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Contracts;

namespace TaskManagementTool.BusinessLogic.Commands.Home.DeleteTodo;

public class DeleteTodoHandler(
    IAuthUtils authUtils,
    ITodoRepository todoRepository,
    IValidator<DeleteTodoRequest> requestValidator
    ) : IRequestHandler<DeleteTodoRequest, DeleteTodoResponse>
{
    public async Task<DeleteTodoResponse> Handle(DeleteTodoRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
        
        if(!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        if (!await authUtils.IsAllowedAction(request.HttpContext, request.TodoId))
        {
            throw new TaskManagementToolException(ApiErrorCode.Forbidden, "");
        }

        await todoRepository.DeleteAsync(entity => entity.Id == request.TodoId);

        return new DeleteTodoResponse { IsSuccess = true };
    }
}