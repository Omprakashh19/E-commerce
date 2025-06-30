using Microsoft.AspNetCore.Http.HttpResults;
using SimpleShop.DTOs;
using SimpleShop.Models.ProductReviewImagesModels;
using SimpleShop.Models.ProductReviewModels;
using SimpleShop.Repositories.Interfaces;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;

        public ProductReviewService(IProductReviewRepository reviewRepository, IProductRepository productRepository)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
        }
        public async Task<bool> AddReviewAsync(int userId, AddReviewRequestDto dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            bool alreadyViewed = await _reviewRepository.HasUserReviewedAsync(userId, dto.ProductId);
            if (alreadyViewed)
            {
                return false;
            }

            var review = new ProductReview
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow,
                Images = new List<ProductReviewImages>()
            };

            if (dto.Images != null && dto.Images.Any())
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/reviews");
                Directory.CreateDirectory(folder);

                foreach (var file in dto.Images)
                {
                    if (file.Length > 0)
                    {
                        var uniqueName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(folder, uniqueName);

                        using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);

                        review.Images.Add(new ProductReviewImages
                        {
                            ImageUrl = $"/images/reviews/{uniqueName}"
                        });
                    }
                }
            }

            await _reviewRepository.AddReviewAsync(review);
            await UpdateProductRatingStatsAsync(dto.ProductId);
            return true;
        }

        public async Task<List<ReviewResponseDto>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
            return reviews.Select(r => new ReviewResponseDto
            {
                Username = r.User.Username,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                ImageUrls = r.Images.Select(i => i.ImageUrl).ToList()
            }).ToList();
        }

        public async Task<bool> UpdateReviewAsync(int userId, UpdateReviewRequestDto dto)
        {
            var existing = await _reviewRepository.GetUserReviewAsync(userId, dto.ProductId);
            if (existing == null) return false;

            existing.Comment = dto.Comment;
            existing.Rating = dto.Rating;
            existing.UpdatedAt = DateTime.UtcNow;

            if (dto.NewImages != null && dto.NewImages.Any())
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/reviews");
                Directory.CreateDirectory(folder);

                foreach (var file in dto.NewImages)
                {
                    if (file.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(folder, uniqueFileName);

                        using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);

                        existing.Images.Add(new ProductReviewImages
                        {
                            ImageUrl = $"/images/reviews/{uniqueFileName}"
                        });
                    }
                }
            }

            bool updated = await _reviewRepository.UpdateReviewAsync(existing);
            if (updated)
                await UpdateProductRatingStatsAsync(dto.ProductId);

            return updated;
        }

        public async Task<bool> DeleteReviewAsync(int userId, int productId)
        {
            var existing = await _reviewRepository.GetUserReviewAsync(userId, productId);
            if (existing == null) return false;

            bool deleted = await _reviewRepository.DeleteReviewAsync(existing);

            if (deleted)
                await UpdateProductRatingStatsAsync(productId);

            return deleted;
        }

        private async Task UpdateProductRatingStatsAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return;

            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);

            if (reviews.Any())
            {
                product.AverageRating = reviews.Average(r => r.Rating);
                product.ReviewCount = reviews.Count;
            }
            else
            {
                product.AverageRating = 0;
                product.ReviewCount = 0;
            }

            await _productRepository.UpdateAsync(product);
        }

        public async Task<bool> AddImagesToReviewAsync(int reviewId, List<IFormFile> images)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null) return false;

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/reviews");
            Directory.CreateDirectory(folder);

            foreach (var file in images)
            {
                if (file.Length > 0)
                {
                    var uniqueName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(folder, uniqueName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    review.Images.Add(new ProductReviewImages
                    {
                        ImageUrl = $"/images/reviews/{uniqueName}"
                    });
                }
            }

            return await _reviewRepository.UpdateReviewAsync(review);
        }

        public async Task<bool> DeleteReviewImageAsync(int imageId)
        {
            return await _reviewRepository.DeleteReviewImageAsync(imageId);
        }



    }
}