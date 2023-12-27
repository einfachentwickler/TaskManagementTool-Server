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
public class AuthController(IAuthHandler handler) : ControllerBase
{
    [HttpPost("register")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        UserManagerResponse result = await handler.RegisterUserAsync(model);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        UserManagerResponse result = await handler.LoginUserAsync(model);

        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }
}