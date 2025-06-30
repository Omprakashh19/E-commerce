using Microsoft.EntityFrameworkCore;
using SimpleShop.Data;
using SimpleShop.Models.ProductReviewModels;
using SimpleShop.Repositories.Interfaces;

namespace SimpleShop.Repositories
{
    public class ProductReviewRepository : IProductReviewRepository
    {

        private readonly AppDbContext _context;
        public ProductReviewRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> HasUserReviewedAsync(int userId, int productId)
        {
            return await _context.productReviews.AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }

        public async Task AddReviewAsync(ProductReview review)
        {
            _context.productReviews.Add(review);
            await _context.SaveChangesAsync();

        }


        public async Task<List<ProductReview>> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = _context.productReviews
                            .Include(r => r.User)
                            .Include(r => r.Images) 
                            .Where(r => r.ProductId == productId)
                            .OrderByDescending(r => r.CreatedAt)
                            .ToListAsync();

            return await reviews;
        }

        public async Task<ProductReview?> GetUserReviewAsync(int userId, int productId)
        {
            return await _context.productReviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);
        }
        public async Task<ProductReview?> GetReviewByIdAsync(int reviewId)
        {
            return await _context.productReviews
                .Include(r => r.Images) // INCLUDE IMAGES!
                .FirstOrDefaultAsync(r => r.Id == reviewId);
        }

        public async Task<bool> UpdateReviewAsync(ProductReview review)
        {
            _context.productReviews.Update(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReviewAsync(ProductReview review)
        {
            _context.productReviews.Remove(review);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteReviewImageAsync(int imageId)
        {
            var image = await _context.productReviewImages.FindAsync(imageId);
            if (image == null) return false;

            _context.productReviewImages.Remove(image);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}