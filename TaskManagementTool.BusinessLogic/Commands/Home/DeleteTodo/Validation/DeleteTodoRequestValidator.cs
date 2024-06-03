using FluentValidation;
using TaskManagementTool.BusinessLogic.Commands.Home.DeleteTodo.Models;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Commands.Home.DeleteTodo.Validation;

public class DeleteTodoRequestValidator : AbstractValidator<DeleteTodoRequest>
{
    public DeleteTodoRequestValidator()
    {
        RuleFor(request => request.TodoId).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.HttpContext).NotNull().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
    }
}