using Application.Commands.Auth.Register.Models;
using FluentValidation;
using Shared.Constants;

namespace Application.Commands.Auth.Register.Validation;

public class UserRegisterCommandValidator : AbstractValidator<UserRegisterCommand>
{
    public UserRegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
                .WithErrorCode(nameof(UserRegisterErrorCode.InvalidEmail))
                .WithMessage(UserRegisterErrorMessages.InvalidEmail)
            .Must(email => email.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE)
                .WithErrorCode(nameof(UserRegisterErrorCode.InvalidEmail))
                .WithMessage(UserRegisterErrorMessages.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithErrorCode(nameof(UserRegisterErrorCode.InvalidPassword))
                .WithMessage(UserRegisterErrorMessages.InvalidPassword)
            .MinimumLength(8)
                .WithErrorCode(nameof(UserRegisterErrorCode.WeakPassword))
                .WithMessage(UserRegisterErrorMessages.WeakPassword)
            .Must(password => password.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE)
                .WithErrorCode(nameof(UserRegisterErrorCode.TextLengthExceeded))
                .WithMessage(UserRegisterErrorMessages.TextLengthExceeded);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
                .WithErrorCode(nameof(UserRegisterErrorCode.InvalidConfirmPassword))
                .WithMessage(UserRegisterErrorMessages.InvalidConfirmPassword)
            .MinimumLength(8)
                .WithErrorCode(nameof(UserRegisterErrorCode.WeakPassword))
                .WithMessage(UserRegisterErrorMessages.WeakPassword)
            .Equal(x => x.Password)
                .WithErrorCode(nameof(UserRegisterErrorCode.ConfirmPasswordDoesNotMatch))
                .WithMessage(UserRegisterErrorMessages.ConfirmPasswordDoesNotMatch)
            .Must(confirmPassword => confirmPassword.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE)
                .WithErrorCode(nameof(UserRegisterErrorCode.TextLengthExceeded))
                .WithMessage(UserRegisterErrorMessages.TextLengthExceeded);

        RuleFor(x => x.FirstName)
            .NotEmpty()
                .WithErrorCode(nameof(UserRegisterErrorCode.EmptyFirstName))
                .WithMessage(UserRegisterErrorMessages.EmptyFirstName)
            .Must(firstName => firstName.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE)
                .WithErrorCode(nameof(UserRegisterErrorCode.TextLengthExceeded))
                .WithMessage(UserRegisterErrorMessages.TextLengthExceeded);

        RuleFor(x => x.LastName)
            .NotEmpty()
                .WithErrorCode(nameof(UserRegisterErrorCode.EmptyLastName))
                .WithMessage(UserRegisterErrorMessages.EmptyLastName)
            .Must(lastName => lastName.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE)
                .WithErrorCode(nameof(UserRegisterErrorCode.TextLengthExceeded))
                .WithMessage(UserRegisterErrorMessages.TextLengthExceeded);
    }
}