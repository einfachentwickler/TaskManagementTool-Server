using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
    public async Task<IActionResult> GetAll()
    {
        string userId = _authUtils.GetUserId(_httpContextAccessor.HttpContext);

        IEnumerable<TodoDto> messages = await _todoService.GetAsync(SearchCriteriaEnum.GetById, userId);

        return Ok(messages);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        TodoDto todo = await _todoService.FindByIdAsync(id);
        if (todo is null)
        {
            return NotFound(id);
        }

        if (!await _authUtils.IsAllowedAction(_httpContextAccessor.HttpContext, id))
        {
            return Forbid();
        }

        return Ok(todo);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateTodoDto model)
    {
        model.CreatorId = _authUtils.GetUserId(_httpContextAccessor.HttpContext);
        await _todoService.AddAsync(model);
        return Ok(model);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateTodoDto model)
    {
        if (!await _authUtils.IsAllowedAction(_httpContextAccessor.HttpContext, model.Id))
        {
            return Forbid();
        }
        await _todoService.UpdateAsync(model);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        TodoDto model = await _todoService.FindByIdAsync(id);

        if (model is null)
        {
            return NotFound(id);
        }

        if (!await _authUtils.IsAllowedAction(_httpContextAccessor.HttpContext, id))
        {
            return Forbid();
        }

        await _todoService.DeleteAsync(id);
        return Ok(model);
    }
}