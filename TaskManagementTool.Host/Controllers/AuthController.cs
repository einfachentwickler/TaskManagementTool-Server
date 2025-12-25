using Application.Commands.Auth.Login.Models;
using Application.Commands.Auth.Register.Models;
using Application.Commands.Auth.ResetPassword.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
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
    public async Task<IActionResult> Register([FromBody][Required] UserRegisterRequest request)
    {
        UserRegisterResponse result = await mediator.Send(request);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Login([FromBody][Required] UserLoginRequest request)
    {
        UserLoginResponse result = await mediator.Send(request);

        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("reset-password")]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> ResetPassword([FromBody][Required] ResetPasswordRequest request)
    {
        ResetPasswordResponse result = await mediator.Send(request);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}