using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Http;

public class HttpContextDataExtractor(ITaskManagementToolDbContext dbContext) : IHttpContextDataExtractor
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;

    public string GetUserNameIdentifier(HttpContext context)
    {
        return context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }

    public async Task<bool> IsAllowedActionAsync(HttpContext context, int todoId, CancellationToken cancellationToken)
    {
        string userId = GetUserNameIdentifier(context);

        var todo = await _dbContext.Todos.FirstOrDefaultAsync(x => x.Id == todoId, cancellationToken);

        return todo is not null && todo.Creator.Id == userId;
    }
}