using SimpleShop.DTOs.CartDto;
using SimpleShop.Models.CartModels;
using SimpleShop.Repositories.Interfaces;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartDto?> GetCartAsync(int userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }
        
        public async Task AddItemAsync(int userId, int productId, int quantity)
        {
            await _cartRepository.AddItemAsync(userId, productId, quantity);
        }

        public async Task RemoveItemAsync(int userId, int productId)
        {
            await _cartRepository.RemoveItemAsync(userId, productId);
        }

        public async Task UpdateQuantityAsync(int userId, int productId, int quantity)
        {
            await _cartRepository.UpdateQuantityAsync(userId, productId, quantity);
        }
    }
}
