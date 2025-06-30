namespace SimpleShop.DTOs
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = null!;
        public List<string> ImageUrls { get; set; } = new();
        public int StockQuantity { get; set; }

        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }

        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }


    }
}
