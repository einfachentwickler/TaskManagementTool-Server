using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Contracts;

namespace TaskManagementTool.BusinessLogic.Commands.Utils;

public interface IAuthUtils
{
    public string GetUserId(HttpContext context);

    public Task<bool> IsAllowedAction(ITodoRepository todoRepository, HttpContext context, int todoId);
}