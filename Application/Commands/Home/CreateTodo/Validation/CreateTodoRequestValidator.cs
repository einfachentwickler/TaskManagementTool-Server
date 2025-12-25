using Application.Commands.Home.CreateTodo.Models;
using FluentValidation;
using TaskManagementTool.Common.Enums;

namespace Application.Commands.Home.CreateTodo.Validation;

public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(request => request.HttpContext).NotNull().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.CreateTodoDto).NotNull().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
            .ChildRules(validator =>
            {
                validator.RuleFor(todo => todo.Name).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
                validator.RuleFor(todo => todo.Content).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
            });
    }
}