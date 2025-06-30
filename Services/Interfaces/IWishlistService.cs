using SimpleShop.DTOs;
using SimpleShop.DTOs.WishlistDto;

public interface IWishlistService
{
    Task<List<WishlistDto>> GetUserWishListAsync(int userId);
    Task<bool> AddToWishlistAsync(int userId, int productId);
    Task<bool> RemoveFromWishlistAsync(int userId, int productId);
}