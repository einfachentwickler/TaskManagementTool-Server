using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Handlers.Auth.Login.Models;
using TaskManagementTool.BusinessLogic.Handlers.Auth.Register.Models;
using TaskManagementTool.BusinessLogic.Handlers.Auth.ResetPassword.Models;
using TaskManagementTool.Host.ActionFilters;

namespace TaskManagementTool.Host.Controllers;

[Route("api/auth")]
[ApiController]
[ModelStateFilter]
[Consumes("application/json")]
[Produces("application/json")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        UserRegisterResponse result = await mediator.Send(request);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        UserLoginResponse result = await mediator.Send(request);

        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("reset-password")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        ResetPasswordResponse result = await mediator.Send(request);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}