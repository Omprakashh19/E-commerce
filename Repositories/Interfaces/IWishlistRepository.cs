using SimpleShop.DTOs.WishlistDto;

public interface IWishlistRepository
{
    Task<List<WishlistDto>> GetUserWishListAsync(int userId);
    Task<bool> AddToWishlistAsync(int userId, int productId);
    Task<bool> RemoveFromWishlistAsync(int userId, int productId);
    Task<bool> ExistsAsync(int userId, int productId);
}