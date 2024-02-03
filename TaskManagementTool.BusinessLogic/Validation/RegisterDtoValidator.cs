﻿using FluentValidation;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Validation;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
#warning TODO improve password validation
#warning TODO take password size from configuration
    public RegisterDtoValidator()
    {
        RuleFor(x => x).NotNull().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
            .ChildRules(innerValidator =>
            {
                innerValidator.RuleFor(x => x.Password)
                .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyPassword))
                .MinimumLength(8).WithErrorCode(nameof(ValidationErrorCodes.WeakPassword))
                .Must(password => password.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));

                innerValidator.RuleFor(x => x.ConfirmPassword)
                    .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyPassword))
                    .MinimumLength(8).WithErrorCode(nameof(ValidationErrorCodes.WeakPassword))
                    .Equal(x => x.Password).WithErrorCode(nameof(ValidationErrorCodes.ConfirmPasswordDoesNotMatch))
                    .Must(confirmPassword => confirmPassword.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));

                innerValidator.RuleFor(x => x.Email)
                    .EmailAddress().WithErrorCode(nameof(ValidationErrorCodes.InvalidEmail))
                    .Must(email => email.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));

                innerValidator.RuleFor(x => x.FirstName)
                    .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
                    .Must(firstName => firstName.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));

                innerValidator.RuleFor(x => x.LastName)
                    .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
                    .Must(lastName => lastName.Length <= ValidationConstants.DEFAULT_TEXT_INPUT_SIZE).WithErrorCode(nameof(ValidationErrorCodes.TextLengthExceeded));
            });
    }
}