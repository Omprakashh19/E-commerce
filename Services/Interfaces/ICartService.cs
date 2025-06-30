using SimpleShop.DTOs.CartDto;

namespace SimpleShop.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto?> GetCartAsync(int userId);
        Task AddItemAsync(int userId, int productId, int quantity);
        Task RemoveItemAsync(int userId, int productId);
        Task UpdateQuantityAsync(int userId, int productId, int quantity);
    }
}
