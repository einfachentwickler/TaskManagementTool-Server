﻿using FluentValidation;
using TaskManagementTool.BusinessLogic.Commands.Home.UpdateTodo.Models;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Commands.Home.UpdateTodo.Validation;

public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
{
    public UpdateTodoRequestValidator()
    {
        RuleFor(request => request.HttpContext).NotNull().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
        RuleFor(request => request.UpdateTodoDto).NotNull().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue))
            .ChildRules(validator =>
            {
                validator.RuleFor(todo => todo.Name).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
                validator.RuleFor(todo => todo.Content).NotEmpty().WithErrorCode(nameof(ValidationErrorCodes.EmptyValue));
            });
    }
}