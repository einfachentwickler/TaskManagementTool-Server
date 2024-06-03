using FluentValidation;
using TaskManagementTool.BusinessLogic.Commands.Auth.Login.Models;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Commands.Auth.Login.Validation;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
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