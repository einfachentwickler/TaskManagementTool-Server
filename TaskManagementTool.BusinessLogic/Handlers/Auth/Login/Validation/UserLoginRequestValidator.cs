using FluentValidation;
using TaskManagementTool.BusinessLogic.Handlers.Auth.Login.Models;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Handlers.Auth.Login.Validation;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x).NotNull().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
           .ChildRules(innerValidator =>
           {
               innerValidator.RuleFor(x => x.Password)
               .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyPassword))
               .MinimumLength(8).WithErrorCode(nameof(ValidationErrorCodes.WeakPassword))
               .Must(password => password.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));

               innerValidator.RuleFor(x => x.Email)
                   .EmailAddress().WithErrorCode(nameof(ValidationErrorCodes.InvalidEmail))
                   .Must(email => email.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));
           });
    }
}