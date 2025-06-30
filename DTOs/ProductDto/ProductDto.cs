namespace SimpleShop.DTOs.ProductDto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile> Images { get; set; } = new();
        public int StockQuantity { get; set; }


    }
}
