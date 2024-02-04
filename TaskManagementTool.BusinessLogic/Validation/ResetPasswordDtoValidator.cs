using FluentValidation;
using TaskManagementTool.BusinessLogic.Commands.Auth.ResetPassword.Models;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Validation;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(request => request).NotNull()
            .ChildRules(validator =>
            {
                validator.RuleFor(request => request.Email)
                    .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
                    .EmailAddress()
                    .Must(email => email.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));

                validator.RuleFor(request => request.NewPassword)
                    .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
                    .Must(newPassword => newPassword.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));

                validator.RuleFor(request => request.ConfirmNewPassword)
                    .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
                    .Equal(request => request.NewPassword).WithErrorCode(nameof(ValidationErrorCodes.ConfirmPasswordDoesNotMatch))
                    .Must(confirmNewPassword => confirmNewPassword.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));
            });
    }
}