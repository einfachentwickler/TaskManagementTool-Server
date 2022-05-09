using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.Services.Utils;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Host.ActionFilters;

namespace TaskManagementTool.Host.Controllers
{
    [Route("api/home")]
    [ApiController, Authorize]
    [ModelStateFilter]
    public class HomeController : ControllerBase
    {
        private readonly ITodoService _todoService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IAuthUtils _authUtils;

        public HomeController(ITodoService service, IHttpContextAccessor httpContextAccessor, IAuthUtils utils)
        {
            (_todoService, _httpContextAccessor, _authUtils) = (service, httpContextAccessor, utils);
        }

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
            string userId = _authUtils.GetUserId(_httpContextAccessor.HttpContext);
            model.CreatorId = userId;
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
            return Ok();
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
}
