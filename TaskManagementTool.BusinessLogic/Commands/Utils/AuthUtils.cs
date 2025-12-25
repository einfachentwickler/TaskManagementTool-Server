using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.BusinessLogic.Commands.Utils;

public class AuthUtils(ITaskManagementToolDbContext dbContext) : IAuthUtils
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;

    public string GetUserId(HttpContext context)
    {
        return context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new TaskManagementToolException(ApiErrorCode.Unautorized, "User id was not found in claims");
    }

    public async Task<bool> IsAllowedActionAsync(HttpContext context, int todoId, CancellationToken cancellationToken)
    {
        string userId = GetUserId(context);

        var todo = await _dbContext.Todos.FirstOrDefaultAsync(x => x.Id == todoId, cancellationToken);

        return todo is not null && todo.Creator.Id == userId;
    }
}