using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.DeleteTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodos.Models;
using TaskManagementTool.BusinessLogic.Commands.Home.UpdateTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.Host.ActionFilters;

namespace TaskManagementTool.Host.Controllers;

[Route("api/home")]
[ApiController, Authorize]
[ModelStateFilter]
public class HomeController(ITodoHandler todoHandler, IMediator mediator, IHttpContextAccessor httpContextAccessor, IAuthUtils authUtils, ITodoRepository todoRepository) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<TodoDto>))]
    public async Task<IActionResult> GetTodos([FromQuery][Required] int pageNumber, [FromQuery][Required] int pageSize)
    {
        GetTodosRequest request = new()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            HttpContext = httpContextAccessor.HttpContext
        };

        return Ok(await mediator.Send(request));
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(TodoDto))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetById([FromRoute][Required] int id)
    {
        TodoDto todo = await todoHandler.FindByIdAsync(id);

        if (!await authUtils.IsAllowedAction(todoRepository, httpContextAccessor.HttpContext, id))
        {
            return Forbid();
        }

        return Ok(todo);
    }

    [HttpPost]
    [Consumes("application/json")]
    [SwaggerResponse((int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody][Required] CreateTodoDto model)
    {
        model.CreatorId = authUtils.GetUserId(httpContextAccessor.HttpContext);
        return Ok(await todoHandler.CreateAsync(model));
    }

    [HttpPut]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    [Consumes("application/json")]
    public async Task<IActionResult> Update([FromBody][Required] UpdateTodoDto request)
    {
        UpdateTodoRequest req = new()
        {
            HttpContext = httpContextAccessor.HttpContext,
            UpdateTodoDto = request
        };

        UpdateTodoResponse response = await mediator.Send(req);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> Delete([FromRoute][Required] int id)
    {
        DeleteTodoRequest request = new()
        {
            TodoId = id,
            HttpContext = httpContextAccessor.HttpContext
        };

        DeleteTodoResponse response = await mediator.Send(request);

        return Ok(response);
    }
}