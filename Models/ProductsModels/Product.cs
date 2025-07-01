
namespace SimpleShop.Models.ProductsModels
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<ProductImage> Images { get; set; }
        public double AverageRating { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;
        public decimal? DiscountPercentage { get; set; }  
        public decimal? DiscountAmount { get; set; }
    }
}
