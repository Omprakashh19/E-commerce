using SimpleShop.Models;

namespace SimpleShop.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
         Task<User?> GetByEmailAsync(string email);  // <-- New
        Task AddUserAsync(User user);
    }
}
