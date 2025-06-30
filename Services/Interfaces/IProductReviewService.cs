using SimpleShop.DTOs;
using SimpleShop.Models.ProductReviewModels;

namespace SimpleShop.Services.Interfaces
{
    public interface IProductReviewService
    {
        Task<bool> AddReviewAsync(int userId, AddReviewRequestDto dto);
        Task<List<ReviewResponseDto>> GetProductReviewsAsync(int productId);
        Task<bool> UpdateReviewAsync(int userId, UpdateReviewRequestDto dto); // ✅ FIX
        Task<bool> DeleteReviewAsync(int userId, int productId);
        Task<bool> AddImagesToReviewAsync(int reviewId, List<IFormFile> images);
        Task<bool> DeleteReviewImageAsync(int imageId);
    }
}