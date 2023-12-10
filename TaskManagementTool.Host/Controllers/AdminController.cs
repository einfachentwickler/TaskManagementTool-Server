using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
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
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [Consumes("application/json")]
    public async Task<IActionResult> GetUsers()
    {
        IEnumerable<UserDto> users = await _adminHandler.GetUsersAsync();
        return Ok(users);
    }

    [HttpPost("reverse-status/{id}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ReverseStatus([FromRoute] string id)
    {
        await _adminHandler.BlockOrUnblockUserAsync(id);
        return NoContent();
    }

    [HttpDelete("users/{id}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteUser([FromRoute] string id)
    {
        await _adminHandler.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpGet("todos")]
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTodos()
    {
        IEnumerable<TodoDto> todos = await _todoHandler.GetAsync(SearchCriteriaEnum.GetAll);
        return Ok(todos);
    }

    [HttpDelete("todos/{id:int}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        await _todoHandler.DeleteAsync(id);
        return NoContent();
    }
}