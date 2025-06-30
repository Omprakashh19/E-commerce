using SimpleShop.DTOs;
using SimpleShop.Models.ProductReviewModels;

namespace SimpleShop.Repositories.Interfaces
{
    public interface IProductReviewRepository
    {
        Task<bool> HasUserReviewedAsync(int userId, int productId);
        Task AddReviewAsync(ProductReview review);
        Task<List<ProductReview>> GetReviewsByProductIdAsync(int productId);
        Task<ProductReview?> GetUserReviewAsync(int userId, int productId);
        Task<bool> UpdateReviewAsync(ProductReview review);
        Task<bool> DeleteReviewAsync(ProductReview review);
        Task<ProductReview?> GetReviewByIdAsync(int reviewId);
        Task<bool> DeleteReviewImageAsync(int imageId);


    }
}