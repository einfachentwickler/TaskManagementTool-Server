using Application.Commands.Auth.ResetPassword.Models;
using FluentValidation;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Enums;

namespace Application.Commands.Auth.ResetPassword.Validation;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x).NotNull().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
            .ChildRules(innerValidator =>
            {
                innerValidator.RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
                .MinimumLength(8).WithErrorCode(nameof(ValidationErrorCodes.WeakPassword))
                .Must(password => password.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));

                innerValidator.RuleFor(x => x.NewPassword)
               .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
               .MinimumLength(8).WithErrorCode(nameof(ValidationErrorCodes.WeakPassword))
               .Must(password => password.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));

                innerValidator.RuleFor(x => x.ConfirmNewPassword)
                    .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
                    .MinimumLength(8).WithErrorCode(nameof(ValidationErrorCodes.WeakPassword))
                    .Equal(x => x.NewPassword).WithErrorCode(nameof(ValidationErrorCodes.ConfirmPasswordDoesNotMatch))
                    .Must(confirmPassword => confirmPassword.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));

                innerValidator.RuleFor(x => x.Email)
                    .EmailAddress().WithErrorCode(nameof(ValidationErrorCodes.InvalidEmail))
                    .Must(email => email.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));
            });
    }
}
