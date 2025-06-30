using SimpleShop.Models.ProductReviewImagesModels;
using SimpleShop.Models.ProductsModels;

namespace SimpleShop.Models.ProductReviewModels
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public List<ProductReviewImages> Images { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}