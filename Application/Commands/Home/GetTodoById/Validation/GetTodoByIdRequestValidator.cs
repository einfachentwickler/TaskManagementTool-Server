using Application.Commands.Home.GetTodoById.Models;
using FluentValidation;
using TaskManagementTool.Common.Enums;

namespace Application.Commands.Home.GetTodoById.Validation;
public class GetTodoByIdRequestValidator : AbstractValidator<GetTodoByIdRequest>
{
    public GetTodoByIdRequestValidator()
    {
        RuleFor(request => request.HttpContext).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.TodoId).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
    }
}