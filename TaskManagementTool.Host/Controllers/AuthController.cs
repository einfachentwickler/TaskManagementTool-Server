using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;

namespace TaskManagementTool.Host.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _userService;

        public AuthController(IAuthService service) => _userService = service;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserManagerResponse result = await _userService.RegisterUserAsync(model);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserManagerResponse result = await _userService.LoginUserAsync(model);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
