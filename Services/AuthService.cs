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

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<string?> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existing != null)
                return null;

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = dto.Password, // 🔐 Hash this in real apps (e.g., BCrypt)
                Role = dto.Role
            };

            await _userRepository.AddUserAsync(user);
            return GenerateToken(user);
        }

        public async Task<LoginResult> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
            {
                return new LoginResult { ErrorMessage = "Wrong username" };
            }

            if (user.PasswordHash != dto.Password) // 🔐 Replace with hash verification in real apps
            {
                return new LoginResult { ErrorMessage = "Wrong password" };
            }

            var token = GenerateToken(user);
            return new LoginResult { Token = token };
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // 👈 Required to extract userId
                new Claim(ClaimTypes.Name, user.Username),
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
