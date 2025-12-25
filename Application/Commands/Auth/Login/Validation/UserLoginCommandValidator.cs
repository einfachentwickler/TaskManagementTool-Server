using Application.Commands.Auth.Login.Models;
using FluentValidation;
using TaskManagementTool.Common.Constants;

namespace Application.Commands.Auth.Login.Validation;

public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
{
    public UserLoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode(nameof(UserLoginErrorCode.EmptyValue)).WithMessage(UserLoginErrorMessages.InvalidEmail)
            .EmailAddress().WithErrorCode(nameof(UserLoginErrorCode.InvalidEmail)).WithMessage(UserLoginErrorMessages.InvalidEmail)
            .Must(email => email.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(UserLoginErrorCode.LengthExceeded)).WithMessage(UserLoginErrorMessages.LengthExceeded);

        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode(nameof(UserLoginErrorCode.EmptyValue)).WithMessage(UserLoginErrorMessages.PasswordRequired)
            .Must(password => password.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(UserLoginErrorCode.LengthExceeded)).WithMessage(UserLoginErrorMessages.LengthExceeded);
    }
}