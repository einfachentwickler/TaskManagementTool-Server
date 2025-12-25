using Application.Commands.Home.DeleteTodo.Models;
using FluentValidation;
using TaskManagementTool.Common.Enums;

namespace Application.Commands.Home.DeleteTodo.Validation;

public class DeleteTodoRequestValidator : AbstractValidator<DeleteTodoRequest>
{
    public DeleteTodoRequestValidator()
    {
        RuleFor(request => request.TodoId).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.HttpContext).NotNull().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
    }
}