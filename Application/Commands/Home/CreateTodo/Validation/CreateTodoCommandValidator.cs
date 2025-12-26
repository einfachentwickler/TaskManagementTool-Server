using Application.Commands.Home.CreateTodo.Models;
using FluentValidation;
using Shared.Constants;

namespace Application.Commands.Home.CreateTodo.Validation;

public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(request => request.CreateTodoDto)
            .NotNull()
                .WithErrorCode(nameof(CreateTodoErrorCode.InvalidRequest))
                .WithMessage(CreateTodoErrorMessages.InvalidRequest)
            .ChildRules(validator =>
            {
                validator.RuleFor(todo => todo.Name)
                    .NotEmpty()
                        .WithErrorCode(nameof(CreateTodoErrorCode.InvalidName))
                        .WithMessage(CreateTodoErrorMessages.InvalidName)
                      .Must(password => password.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE)
                        .WithErrorCode(nameof(CreateTodoErrorCode.InvalidName))
                        .WithMessage(CreateTodoErrorMessages.InvalidName);

                validator.RuleFor(todo => todo.Content)
                    .NotEmpty()
                        .WithErrorCode(nameof(CreateTodoErrorCode.InvalidContent))
                        .WithMessage(CreateTodoErrorMessages.InvalidContent)
                    .Must(content => content.Length <= ValidationConstants.LARGE_TEXT_INPUT_SIZE)
                        .WithErrorCode(nameof(CreateTodoErrorCode.ContentTooLarge))
                        .WithMessage(CreateTodoErrorMessages.ContentTooLarge);
            });
    }
}