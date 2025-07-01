using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SimpleShop.DTOs;
using SimpleShop.Models;
using SimpleShop.Repositories.Interfaces;
using SimpleShop.Services.Interfaces;
using SimpleShop.Helpers.Interfaces;

namespace SimpleShop.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordService _passwordService;
        private readonly IPasswordResetRepository _passwordResetRepository;

        public AuthService(
            IUserRepository userRepository,
            IConfiguration config,
            IEmailSender emailSender,
            IPasswordService passwordService,
            IPasswordResetRepository passwordResetRepository)
        {
            _userRepository = userRepository;
            _config = config;
            _emailSender = emailSender;
            _passwordService = passwordService;
            _passwordResetRepository = passwordResetRepository;
        }

        public async Task<string?> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                return null;

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = _passwordService.HashPassword(dto.Password),
                Role = dto.Role
            };

            await _userRepository.AddUserAsync(user);
            return GenerateToken(user);
        }

        public async Task<LoginResult> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                return new LoginResult { ErrorMessage = "Wrong email" };
            }

            if (!_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
            {
                return new LoginResult { ErrorMessage = "Wrong password" };
            }

            var token = GenerateToken(user);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "LoginSuccess.html");
            string htmlBody = await File.ReadAllTextAsync(templatePath);

            htmlBody = htmlBody.Replace("{{USERNAME}}", user.Username);
            await _emailSender.SendEmailAsync(user.Email, "Login Successful", htmlBody);

            return new LoginResult { Token = token };
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return false;

            var existing = await _passwordResetRepository.GetByEmailAsync(email);
            if (existing != null)
            {
                await _passwordResetRepository.DeleteAsync(existing);
            }

            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var reset = new PasswordReset
            {
                Email = email,
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddHours(1)
            };

            await _passwordResetRepository.AddAsync(reset);

            var link = $"http://localhost:5019/api/auth/reset-password?token={Uri.EscapeDataString(token)}";


            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ResetPassword.html");
            string htmlBody = await File.ReadAllTextAsync(templatePath);

            // Replace placeholder with actual link
            htmlBody = htmlBody.Replace("{{RESET_LINK}}", link);

            await _emailSender.SendEmailAsync(email, "Reset Your Password", htmlBody);


            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var reset = await _passwordResetRepository.GetByTokenAsync(token);
            if (reset == null || reset.ExpiryDate < DateTime.UtcNow) return false;

            var user = await _userRepository.GetByEmailAsync(reset.Email);
            if (user == null) return false;

            user.PasswordHash = _passwordService.HashPassword(newPassword);
            await _userRepository.UpdateUserAsync(user);

            await _passwordResetRepository.DeleteAsync(reset);

            return true;
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
