using SimpleShop.DTOs;

namespace SimpleShop.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> RegisterAsync(RegisterDto dto);
        Task<LoginResult> LoginAsync(LoginDto dto);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
}
