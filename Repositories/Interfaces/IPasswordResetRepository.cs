using SimpleShop.Models;

namespace SimpleShop.Repositories.Interfaces
{
    public interface IPasswordResetRepository
    {
        Task AddAsync(PasswordReset reset);
        Task<PasswordReset?> GetByTokenAsync(string token);
        Task<PasswordReset?> GetByEmailAsync(string email);
        Task DeleteAsync(PasswordReset reset);
    }
}
