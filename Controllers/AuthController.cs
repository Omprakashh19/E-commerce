using Microsoft.AspNetCore.Mvc;
using SimpleShop.DTOs;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
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
        [HttpPost("forgot-password")]
        public async Task<IActionResult> Forgot([FromBody] ForgotPasswordDto dto)
        {
            var result = await _authService.ForgotPasswordAsync(dto.Email);
            return Ok(result ? "Check your email." : "Invalid email.");
        }

         // ✅ GET: Render form
        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid token.");
            }

            ViewBag.Token = token;
            return View();
        }

        // ✅ POST: Handle form submit
        [HttpPost("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string token, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Invalid token.");
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                ViewBag.Token = token;
                return View();
            }

            var success = await _authService.ResetPasswordAsync(token, newPassword);
            if (!success)
            {
                ModelState.AddModelError("", "Invalid or expired token.");
                ViewBag.Token = token;
                return View();
            }

            return RedirectToAction("ResetPasswordSuccess");
        }

        [HttpGet("reset-password-success")]
        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }


    }
}
