using Application.Commands.Auth.Login.Models;
using Application.Commands.Auth.RefreshToken.Models;
using Application.Commands.Auth.Register.Models;
using Application.Commands.Auth.ResetPassword.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Constants;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Controllers;

[Route("api/auth")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[EnableRateLimiting(RateLimiterConstants.CONCURRENCY_POLICY_NAME)]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserRegisterCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserLoginCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}