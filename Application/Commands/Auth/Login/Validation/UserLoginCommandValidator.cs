using Application.Commands.Auth.Login.Models;
using FluentValidation;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Enums;

namespace Application.Commands.Auth.Login.Validation;

public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
{
    public UserLoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
            .EmailAddress().WithErrorCode(nameof(ValidationErrorCodes.InvalidEmail))
            .Must(email => email.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));

        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
            .Must(password => password.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));
    }
}