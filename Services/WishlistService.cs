using SimpleShop.DTOs.WishlistDto;

public class WishlistService : IWishlistService
{

    private readonly IWishlistRepository _wishlistRepository;

    public WishlistService(IWishlistRepository wishlistRepository)
    {
        _wishlistRepository = wishlistRepository;
    }
    public async Task<bool> AddToWishlistAsync(int userId, int productId)
    {
        return await _wishlistRepository.AddToWishlistAsync(userId, productId);
    }

    public async Task<List<WishlistDto>> GetUserWishListAsync(int userId)
    {
        return await _wishlistRepository.GetUserWishListAsync(userId);
    }

    public async Task<bool> RemoveFromWishlistAsync(int userId, int productId)
    {
        return await _wishlistRepository.RemoveFromWishlistAsync(userId, productId);
    }
}