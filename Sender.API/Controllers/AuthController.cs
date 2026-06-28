using Microsoft.AspNetCore.Mvc;
using Sender.API.DTO;
using Sender.Application;

namespace Sender.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private readonly AuthUser _authUser;

        public AuthController(AuthUser authUser)
        {
            _authUser = authUser;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authUser.RegisterAsync(request.Username, request.Email, request.Password);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return Ok(ApiResponse<object>.SuccessResult(new { Token = result.Value }));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authUser.LoginAsync(request.Email, request.Password);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return Ok(ApiResponse<object>.SuccessResult(new { Token = result.Value }));
        }
    }
}

