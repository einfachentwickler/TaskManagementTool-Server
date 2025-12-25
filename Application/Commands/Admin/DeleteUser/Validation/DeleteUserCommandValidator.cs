using Application.Commands.Admin.DeleteUser.Models;
using FluentValidation;

namespace Application.Commands.Admin.DeleteUser.Validation;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode(nameof(DeleteUserErrorCode.InvalidEmail)).WithMessage("Email is required.")
            .EmailAddress().WithErrorCode(nameof(DeleteUserErrorCode.InvalidEmail)).WithMessage("A valid email is required.");
    }
}