using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.Services.Utils;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Host.ActionFilters;

namespace TaskManagementTool.Host.Controllers;

[Route("api/home")]
[ApiController, Authorize]
[ModelStateFilter]
public class HomeController(ITodoHandler service, IHttpContextAccessor httpContextAccessor, IAuthUtils utils) : ControllerBase
{
    private readonly ITodoHandler _todoService = service;

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private readonly IAuthUtils _authUtils = utils;

    [HttpGet]
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<TodoDto>))]
    public async Task<IActionResult> GetAll()
    {
        string userId = _authUtils.GetUserId(_httpContextAccessor.HttpContext);

        IEnumerable<TodoDto> messages = await _todoService.GetAsync(SearchCriteriaEnum.GetById, userId);

        return Ok(messages);
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(TodoDto))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        TodoDto todo = await _todoService.FindByIdAsync(id);

        if (!await _authUtils.IsAllowedAction(_httpContextAccessor.HttpContext, id))
        {
            return Forbid();
        }

        return Ok(todo);
    }

    [HttpPost]
    [Consumes("application/json")]
    [SwaggerResponse((int)HttpStatusCode.Created)]
    public async Task<IActionResult> Add([FromBody] CreateTodoDto model)
    {
        model.CreatorId = _authUtils.GetUserId(_httpContextAccessor.HttpContext);
        await _todoService.AddAsync(model);
        return Created();
    }

    [HttpPut]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    [Consumes("application/json")]
    public async Task<IActionResult> Update([FromBody] UpdateTodoDto model)
    {
        if (!await _authUtils.IsAllowedAction(_httpContextAccessor.HttpContext, model.Id))
        {
            return Forbid();
        }
        await _todoService.UpdateAsync(model);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!await _authUtils.IsAllowedAction(_httpContextAccessor.HttpContext, id))
        {
            return Forbid();
        }

        await _todoService.DeleteAsync(id);
        return NoContent();
    }
}