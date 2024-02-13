﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Admin.DeleteTodoByAdmin.Models;
using TaskManagementTool.BusinessLogic.Commands.Admin.DeleteUser.Models;
using TaskManagementTool.BusinessLogic.Commands.Admin.GetTodos.Models;
using TaskManagementTool.BusinessLogic.Commands.Admin.GetUsers.Models;
using TaskManagementTool.BusinessLogic.Commands.Admin.ReverseStatus.Models;

namespace TaskManagementTool.Host.Controllers;

[Route("api/admin/")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController(IMediator mediator) : ControllerBase
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
        DeleteUserRequest request = new() { Email = email };

        await mediator.Send(request);

        return NoContent();
    }

    [HttpGet("todos")]
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTodos([FromQuery][Required] int pageNumber, [FromQuery][Required] int pageSize)
    {
        GetTodosByAdminRequest request = new() { PageNumber = pageNumber, PageSize = pageSize };

        GetTodosByAdminResponse response = await mediator.Send(request);

        return Ok(response);
    }

    [HttpDelete("todos/{id:int}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteTodo([FromRoute][Required] int id)
    {
        DeleteTodoByAdminRequest request = new() { TodoId = id };

        await mediator.Send(request);

        return NoContent();
    }
}