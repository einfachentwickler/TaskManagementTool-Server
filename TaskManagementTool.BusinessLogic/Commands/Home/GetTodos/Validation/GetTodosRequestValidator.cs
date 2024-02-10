using FluentValidation;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodos.Models;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Commands.Home.GetTodos.Validation;

public class GetTodosRequestValidator : AbstractValidator<GetTodosRequest>
{
    public GetTodosRequestValidator()
    {
        RuleFor(request => request.PageNumber).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.PageSize).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.HttpContext).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
    }
}