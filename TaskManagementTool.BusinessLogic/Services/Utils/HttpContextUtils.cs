using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.BusinessLogic.Services.Utils
{
    public static class HttpContextUtils
    {
        public static string GetUserId(HttpContext context)
        {
            string userId = context?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? throw new TaskManagementToolException("User id is null");

            return userId;
        }
    }
}
