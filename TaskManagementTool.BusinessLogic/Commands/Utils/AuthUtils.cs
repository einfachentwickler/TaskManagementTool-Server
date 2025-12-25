using Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.BusinessLogic.Commands.Utils;

public class AuthUtils : IAuthUtils
{
    public string GetUserId(HttpContext context)
    {
        return context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new TaskManagementToolException(ApiErrorCode.Unautorized, "User id was not found in claims");
    }

    public async Task<bool> IsAllowedAction(ITodoRepository todoRepository, HttpContext context, int todoId)
    {
        string userId = GetUserId(context);

        var todo = await todoRepository.FirstOrDefaultAsync(todoId);

        return todo is not null && todo.Creator.Id == userId;
    }
}