using FluentValidation;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodoById.Models;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Commands.Home.GetTodoById.Validation;
public class GetTodoByIdRequestValidator : AbstractValidator<GetTodoByIdRequest>
{
    public GetTodoByIdRequestValidator()
    {
        RuleFor(request => request.HttpContext).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.TodoId).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
    }
}