using Microsoft.AspNetCore.Mvc;
using SimpleShop.DTOs;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var token = await _authService.RegisterAsync(dto);
            if (token == null) return BadRequest("Username already exists");
            return Ok(new { token, status = true, message = "User Registration Succesfully Completed." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.IsSuccess)
            {
                return Unauthorized(new
                {
                    status = false,
                    message = result.ErrorMessage
                });
            }

            return Ok(new
            {
                token = result.Token,
                status = true,
                message = "User Login Successfully Completed."
            });
        }

    }
}
