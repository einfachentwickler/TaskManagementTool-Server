using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.Host.ActionFilters;

namespace TaskManagementTool.Host.Controllers;

[Route("api/auth")]
[ApiController]
[ModelStateFilter]
[Consumes("application/json")]
[Produces("application/json")]
public class AuthController(IAuthHandler service) : ControllerBase
{
    private readonly IAuthHandler _userService = service;

    [HttpPost("register")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
    {
        UserManagerResponse result = await _userService.RegisterUserAsync(model);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto model)
    {
        UserManagerResponse result = await _userService.LoginUserAsync(model);

        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }
}