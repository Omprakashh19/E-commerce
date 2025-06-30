using SimpleShop.Models.ProductReviewModels;
using SimpleShop.Models.ProductsModels;

namespace SimpleShop.Models.ProductReviewImagesModels
{
    public class ProductReviewImages
    {
        public int Id { get; set; }
        public int ProductReviewId { get; set; }
        public ProductReview ProductReview { get; set; } = null!;
        public string ImageUrl { get; set; } = string.Empty;
    }
}