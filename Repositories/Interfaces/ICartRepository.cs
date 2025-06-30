using SimpleShop.DTOs.CartDto;
using SimpleShop.Models.CartModels;

namespace SimpleShop.Repositories.Interfaces
{
  public interface ICartRepository
{
    Task<CartDto?> GetCartByUserIdAsync(int userId);
    Task AddItemAsync(int userId, int productId, int quantity);
    Task RemoveItemAsync(int userId, int productId);
    Task UpdateQuantityAsync(int userId, int productId, int quantity);
}


}
