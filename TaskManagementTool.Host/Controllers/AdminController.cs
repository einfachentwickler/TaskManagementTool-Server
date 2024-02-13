using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Admin.GetUsers.Models;
using TaskManagementTool.BusinessLogic.Commands.Admin.ReverseStatus.Models;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;

namespace TaskManagementTool.Host.Controllers;

[Route("api/admin/")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController(IAdminHandler adminHandler, ITodoHandler todoHandler, IMediator mediator) : ControllerBase
{
    [HttpGet("users")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [Consumes("application/json")]
    public async Task<IActionResult> GetUsers([FromQuery][Required] int pageNumber, [FromQuery][Required] int pageSize)
    {
        GetUsersRequest request = new() { PageNumber = pageNumber, PageSize = pageSize };

        GetUsersResponse response = await mediator.Send(request);

        return Ok(response);
    }

    [HttpPost("reverse-status/{userId}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ReverseStatus([FromRoute][Required] string userId)
    {
        await mediator.Send(new ReverseStatusRequest { UserId = userId });
        return NoContent();
    }

    [HttpDelete("users/{email}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteUser([FromRoute][Required] string email)
    {
        await adminHandler.DeleteAsync(email);
        return NoContent();
    }

    [HttpGet("todos")]
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTodos([FromQuery][Required] int pageNumber, [FromQuery][Required] int pageSize)
    {
        IEnumerable<TodoDto> todos = await todoHandler.GetAsync(pageSize, pageNumber);
        return Ok(todos);
    }

    [HttpDelete("todos/{id:int}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteTodo([FromRoute][Required] int id)
    {
        await todoHandler.DeleteAsync(id);
        return NoContent();
    }
}