using Application.Commands.Auth.ResetPassword.Models;
using FluentValidation;
using Shared.Constants;

namespace Application.Commands.Auth.ResetPassword.Validation;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
                .WithErrorCode(nameof(ResetPasswordErrorCode.InvalidCurrentPassword))
                .WithMessage(ResetPasswordErrorMessages.InvalidCurrentPassword)
            .MinimumLength(8)
                .WithErrorCode(nameof(ResetPasswordErrorCode.InvalidCurrentPassword))
                .WithMessage(ResetPasswordErrorMessages.InvalidCurrentPassword)
            .Must(password => password.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE)
                .WithErrorCode(nameof(ResetPasswordErrorCode.InvalidCurrentPassword))
                .WithMessage(ResetPasswordErrorMessages.InvalidCurrentPassword);

        RuleFor(x => x.NewPassword)
             .NotEmpty()
                .WithErrorCode(nameof(ResetPasswordErrorCode.InvalidNewPassword))
                .WithMessage(ResetPasswordErrorMessages.InvalidNewPassword)
            .MinimumLength(8)
                .WithErrorCode(nameof(ResetPasswordErrorCode.InvalidNewPassword))
                .WithMessage(ResetPasswordErrorMessages.InvalidNewPassword)
            .Must(password => password.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE)
                .WithErrorCode(nameof(ResetPasswordErrorCode.InvalidNewPassword))
                .WithMessage(ResetPasswordErrorMessages.InvalidNewPassword);

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty()
               .WithErrorCode(nameof(ResetPasswordErrorCode.InvalidConfirmNewPassword))
               .WithMessage(ResetPasswordErrorMessages.InvalidConfirmNewPassword)
           .MinimumLength(8)
               .WithErrorCode(nameof(ResetPasswordErrorCode.InvalidConfirmNewPassword))
               .WithMessage(ResetPasswordErrorMessages.InvalidConfirmNewPassword)
           .Must(password => password.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE)
               .WithErrorCode(nameof(ResetPasswordErrorCode.InvalidConfirmNewPassword))
               .WithMessage(ResetPasswordErrorMessages.InvalidConfirmNewPassword)
            .Equal(x => x.NewPassword)
               .WithErrorCode(nameof(ResetPasswordErrorCode.PasswordsDoNotMatch))
               .WithMessage(ResetPasswordErrorMessages.PasswordsDoNotMatch);

        RuleFor(x => x.Email)
            .EmailAddress()
                .WithErrorCode(nameof(ResetPasswordErrorCode.InvalidEmail))
                .WithMessage(ResetPasswordErrorMessages.InvalidEmail)
            .Must(email => email.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE)
                .WithErrorCode(nameof(ResetPasswordErrorCode.InvalidEmail))
                .WithMessage(ResetPasswordErrorMessages.InvalidEmail);
    }
}
