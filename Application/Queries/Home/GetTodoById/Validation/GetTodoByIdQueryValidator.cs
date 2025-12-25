using Application.Queries.Home.GetTodoById.Models;
using FluentValidation;
using TaskManagementTool.Common.Enums;

namespace Application.Queries.Home.GetTodoById.Validation;

public class GetTodoByIdQueryValidator : AbstractValidator<GetTodoByIdQuery>
{
    public GetTodoByIdQueryValidator()
    {
        RuleFor(request => request.HttpContext).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.TodoId).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
    }
}