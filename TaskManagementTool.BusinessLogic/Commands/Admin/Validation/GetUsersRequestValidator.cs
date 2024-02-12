using FluentValidation;
using TaskManagementTool.BusinessLogic.Commands.Admin.Models;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.Validation;
public class GetUsersRequestValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersRequestValidator()
    {
        RuleFor(request => request.PageNumber).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.PageSize).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
    }
}