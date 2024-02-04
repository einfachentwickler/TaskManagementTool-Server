using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Host.ActionFilters;

namespace TaskManagementTool.Host.Controllers;

[Route("api/home")]
[ApiController, Authorize]
[ModelStateFilter]
public class HomeController(ITodoHandler todoHandler, IHttpContextAccessor httpContextAccessor, IAuthUtils authUtils) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<TodoDto>))]
    public async Task<IActionResult> GetUsersTodos([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        string userId = authUtils.GetUserId(httpContextAccessor.HttpContext);

        IEnumerable<TodoDto> todos = await todoHandler.GetAsync(userId, pageNumber, pageSize);

        return Ok(todos);
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(TodoDto))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        TodoDto todo = await todoHandler.FindByIdAsync(id);

        if (!await authUtils.IsAllowedAction(httpContextAccessor.HttpContext, id))
        {
            return Forbid();
        }

        return Ok(todo);
    }

    [HttpPost]
    [Consumes("application/json")]
    [SwaggerResponse((int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody] CreateTodoDto model)
    {
        model.CreatorId = authUtils.GetUserId(httpContextAccessor.HttpContext);
        return Ok(await todoHandler.CreateAsync(model));
    }

    [HttpPut]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    [Consumes("application/json")]
    public async Task<IActionResult> Update([FromBody] UpdateTodoDto model)
    {
        if (!await authUtils.IsAllowedAction(httpContextAccessor.HttpContext, model.Id))
        {
            return Forbid();
        }

        TodoDto updatedTodo = await todoHandler.UpdateAsync(model);

        return Ok(updatedTodo);
    }

    [HttpDelete("{id:int}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!await authUtils.IsAllowedAction(httpContextAccessor.HttpContext, id))
        {
            return Forbid();
        }

        await todoHandler.DeleteAsync(id);
        return NoContent();
    }
}