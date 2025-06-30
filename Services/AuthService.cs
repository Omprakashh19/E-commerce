using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SimpleShop.DTOs;
using SimpleShop.Models;
using SimpleShop.Repositories.Interfaces;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;

        public AuthService(IUserRepository userRepository, IConfiguration config, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _config = config;
            _emailSender = emailSender;
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
                PasswordHash = dto.Password, // 🔐 Hash this in real apps (e.g., BCrypt)
                Role = dto.Role
            };

            await _userRepository.AddUserAsync(user);
            return GenerateToken(user);
        }

        public async Task<LoginResult> LoginAsync(LoginDto dto)
        {
           
            var user = await _userRepository.GetByEmailAsync(dto.Email); // Email is in Username field
            if (user == null)
            {
                return new LoginResult { ErrorMessage = "Wrong email" };
            }

            if (user.PasswordHash != dto.Password) // 🔐 Replace with hash verification in real apps
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

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // 👈 Required to extract userId
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
