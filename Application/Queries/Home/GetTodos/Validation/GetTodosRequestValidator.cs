using Application.Queries.Home.GetTodos.Models;
using FluentValidation;
using TaskManagementTool.Common.Enums;

namespace Application.Queries.Home.GetTodos.Validation;

public class GetTodosRequestValidator : AbstractValidator<GetTodosQuery>
{
    public GetTodosRequestValidator()
    {
        RuleFor(request => request.PageNumber).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.PageSize).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.HttpContext).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
    }
}