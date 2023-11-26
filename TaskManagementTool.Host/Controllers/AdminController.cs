using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.Host.Controllers;

[Route("api/admin/")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController(IAdminHandler adminHandler, ITodoHandler adminService) : ControllerBase
{
    private readonly IAdminHandler _adminHandler = adminHandler;

    private readonly ITodoHandler _todoHandler = adminService;

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        IEnumerable<UserDto> users = await _adminHandler.GetUsersAsync();
        return Ok(users);
    }

    [HttpPost("reverse-status/{id}")]
    public async Task<IActionResult> ReverseStatus(string id)
    {
        UserDto user = await _adminHandler.GetUserAsync(id);

        if (user is null)
        {
            return NotFound(id);
        }

        await _adminHandler.BlockOrUnblockUserAsync(id);
        return Ok(user);
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (await _adminHandler.GetUserAsync(id) is null)
        {
            return NotFound(id);
        }
        await _adminHandler.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpGet("todos")]
    public async Task<IActionResult> GetTodos()
    {
        IEnumerable<TodoDto> todos = await _todoHandler.GetAsync(SearchCriteriaEnum.GetAll);
        return Ok(todos);
    }

    [HttpDelete("todos/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (await _todoHandler.FindByIdAsync(id) is null)
        {
            return NotFound(id);
        }
        await _todoHandler.DeleteAsync(id);
        return Ok(id);
    }
}