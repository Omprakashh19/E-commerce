using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SimpleShop.Data;
using SimpleShop.DTOs.WishlistDto;
using SimpleShop.Models.WishListModels;

public class WishlistRepository : IWishlistRepository
{
    private readonly AppDbContext _context;
    public WishlistRepository(AppDbContext context)
    {
        _context = context;
    }
    

    public async Task<bool> AddToWishlistAsync(int userId, int productId)
    {
        if (await ExistsAsync(userId, productId))
        {
            return false;
        }

        _context.wishlists.Add(new WishList { UserId = userId, ProductId = productId });
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<WishlistDto>> GetUserWishListAsync(int userId)
    {
        return await _context.wishlists
        .Include(w => w.Product)
        .ThenInclude(p => p.Category)
        .Include(w => w.Product.Images)
        .Where(w => w.UserId == userId)
        .Select(w => new WishlistDto
        {
            ProductId = w.ProductId,
            ProductName = w.Product.Name,
            Price = w.Product.Price,
            CategoryName = w.Product.Category.Name,
            ImageUrls = w.Product.Images.Select(i => i.ImageUrl).ToList()
        }).ToListAsync();
    }

    public async Task<bool> RemoveFromWishlistAsync(int userId, int productId)
    {

        var item = await _context.wishlists.FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
        if (item == null)
        {
            return false;
        }

        _context.wishlists.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> ExistsAsync(int userId, int productId)
    {
        return await _context.wishlists.AnyAsync(w => w.UserId == userId && w.ProductId == productId);
    }
}