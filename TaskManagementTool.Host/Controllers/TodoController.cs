using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;

namespace TaskManagementTool.Host.Controllers
{
    [Route("api/home")]
    [ApiController, Authorize]
    public class HomeController : ControllerBase
    {
        private readonly ITodoService _service;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ITodoService service, IHttpContextAccessor httpContextAccessor)
            => (_service, _httpContextAccessor) = (service, httpContextAccessor);

        private async Task<bool> IsAllowedAction(int todoId)
        {
            string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? throw new Exception("User id is null");

            TodoDto todo = await _service.GetSingleAsync(todoId);
            return todo is not null && todo.Creator.Id == userId;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? throw new Exception("User id is null");

            IEnumerable<TodoDto> messages = await _service.GetAsync();

            messages = messages
                .Where(td => td.Creator.Id == userId)
                .OrderByDescending(todo => todo.Importance);

            return Ok(messages);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            TodoDto todo = await _service.GetSingleAsync(id);
            if (todo is null)
            {
                return NotFound(id);
            }

            if (!await IsAllowedAction(id))
            {
                return Forbid();
            }
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateTodoDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User id is null");

            model.CreatorId = userId;
            await _service.AddAsync(model);
            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTodoDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await IsAllowedAction(model.Id))
            {
                return Forbid();
            }
            await _service.UpdateAsync(model);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            TodoDto model = await _service.GetSingleAsync(id);
            if (model is null)
            {
                return NotFound(id);
            }
            if (!await IsAllowedAction(id))
            {
                return Forbid();
            }
            await _service.DeleteAsync(id);
            return Ok(model);
        }
    }
}
