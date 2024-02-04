﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.BusinessLogic.Commands.Utils;

public class AuthUtils(ITodoHandler service) : IAuthUtils
{
    public string GetUserId(HttpContext context)
    {
        return context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new TaskManagementToolException(ApiErrorCode.Unautorized, "User id was not found in claims");
    }

    public async Task<bool> IsAllowedAction(HttpContext context, int todoId)
    {
        string userId = GetUserId(context);

        TodoDto todo = await service.FindByIdAsync(todoId);

        return todo is not null && todo.Creator.Id == userId;
    }
}