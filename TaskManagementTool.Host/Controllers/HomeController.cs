﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.CreateTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Home.DeleteTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodoById.Models;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodos.Models;
using TaskManagementTool.BusinessLogic.Commands.Home.UpdateTodo.Models;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Host.ActionFilters;

namespace TaskManagementTool.Host.Controllers;

[Route("api/home")]
[ApiController, Authorize]
[ModelStateFilter]
public class HomeController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : ControllerBase
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
        GetTodoByIdRequest request = new()
        {
            TodoId = id,
            HttpContext = httpContextAccessor.HttpContext
        };

        GetTodoByIdResponse response = await mediator.Send(request);

        return Ok(response);
    }

    [HttpPost]
    [Consumes("application/json")]
    [SwaggerResponse((int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody][Required] CreateTodoDto model)
    {
        CreateTodoRequest request = new()
        {
            HttpContext = httpContextAccessor.HttpContext,
            CreateTodoDto = model
        };

        CreateTodoResponse response = await mediator.Send(request);

        return Ok(response);
    }

    [HttpPut]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    [Consumes("application/json")]
    public async Task<IActionResult> Update([FromBody][Required] UpdateTodoDto model)
    {
        UpdateTodoRequest req = new()
        {
            HttpContext = httpContextAccessor.HttpContext,
            UpdateTodoDto = model
        };

        UpdateTodoResponse response = await mediator.Send(req);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
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