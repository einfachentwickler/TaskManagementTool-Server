using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagementTool.BusinessLogic.Commands.Utils;

public interface IAuthUtils
{
    public string GetUserId(HttpContext context);

    public Task<bool> IsAllowedActionAsync(HttpContext context, int todoId, CancellationToken cancellationToken);
}