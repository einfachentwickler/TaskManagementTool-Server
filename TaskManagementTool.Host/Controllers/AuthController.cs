using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.Host.ActionFilters;

namespace TaskManagementTool.Host.Controllers;

[Route("api/auth")]
[ApiController]
[ModelStateFilter]
public class AuthController(IAuthService service) : ControllerBase
{
    private readonly IAuthService _userService = service;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
    {
        UserManagerResponse result = await _userService.RegisterUserAsync(model);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto model)
    {
        UserManagerResponse result = await _userService.LoginUserAsync(model);

        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }
}