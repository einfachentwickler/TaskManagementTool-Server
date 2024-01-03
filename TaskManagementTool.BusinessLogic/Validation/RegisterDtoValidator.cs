using FluentValidation;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Validation;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
#warning TODO improve password validation
#warning TODO take password size from configuration
#warning TODO write unit tests
    public RegisterDtoValidator()
    {
        RuleFor(x => x).NotNull().WithErrorCode(nameof(ValidationErrorCodes.EmptyBody))
            .ChildRules(innerValidator =>
            {
                innerValidator.RuleFor(x => x.Password)
                    .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyPassword))
                    .MinimumLength(8).WithErrorCode(nameof(ValidationErrorCodes.WeakPassword));

                innerValidator.RuleFor(x => x.ConfirmPassword)
                    .NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyPassword))
                    .MinimumLength(8).WithErrorCode(nameof(ValidationErrorCodes.WeakPassword))
                    .Equal(x => x.Password).WithErrorCode(nameof(ValidationErrorCodes.ConfirmPasswordDoesNotMatch));

                innerValidator.RuleFor(x => x.Email)
                    .EmailAddress().WithErrorCode(nameof(ValidationErrorCodes.InvalidEmail));

                innerValidator.RuleFor(x => x.FirstName).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyName));
                innerValidator.RuleFor(x => x.LastName).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyName));
            });

    }
}