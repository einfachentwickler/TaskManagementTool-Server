using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.ViewModels;

namespace TaskManagementTool.Host.Controllers
{
    [Route("api/admin/")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        private readonly ITodoService _todoService;

        public AdminController(IAdminService service, ITodoService todoService)
            => (_adminService, _todoService) = (service, todoService);

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            IEnumerable<UserDto> users = await _adminService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost("reverse-status/{id}")]
        public async Task<IActionResult> ReverseStatus(string id)
        {
            UserDto user = await _adminService.GetUserAsync(id);

            if (user is null)
            {
                return NotFound(id);
            }
            user.IsBlocked = !user.IsBlocked;
            await _adminService.UpdateUserAsync(user);
            return Ok(user);
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (await _adminService.GetUserAsync(id) is null)
            {
                return NotFound(id);
            }
            await _adminService.DeleteUserAsync(id);
            return Ok();
        }

        [HttpGet("todos")]
        public async Task<IActionResult> GetTodos()
        {
            IEnumerable<TodoDto> todos = await _todoService.GetAsync();
            return Ok(todos);
        }

        [HttpDelete("todos/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _todoService.FirstAsync(id) is null)
            {
                return NotFound(id);
            }
            await _todoService.DeleteAsync(id);
            return Ok(id);
        }
    }
}
