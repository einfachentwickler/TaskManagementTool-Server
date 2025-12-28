using Application.Commands.Home.UpdateTodo.Models;
using FluentValidation;

namespace Application.Commands.Home.UpdateTodo.Validation;

public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidator()
    {
        RuleFor(todo => todo.Name)
            .NotEmpty()
                .WithErrorCode(nameof(UpdateTodoErrorCode.InvalidName))
                .WithMessage(UpdateTodoErrorMessages.InvalidName);

        RuleFor(todo => todo.Content)
            .NotEmpty()
                .WithErrorCode(nameof(UpdateTodoErrorCode.InvalidContent))
                .WithMessage(UpdateTodoErrorMessages.InvalidContent);
    }
}