using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.BusinessLogic.Services.Utils
{
    public class AuthUtils : IAuthUtils
    {
        private readonly ITodoService _service;

        public AuthUtils(ITodoService service)
        {
            _service = service;
        }

        public string GetUserId(HttpContext context)
        {
            string userId = context?.User
                                .FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? throw new TaskManagementToolException("User id is null");

            return userId;
        }

        public async Task<bool> IsAllowedAction(HttpContext context, int todoId)
        {
            string userId = GetUserId(context);

            TodoDto todo = await _service.FindByIdAsync(todoId);

            return todo is not null && todo.Creator.Id == userId;
        }
    }
}
