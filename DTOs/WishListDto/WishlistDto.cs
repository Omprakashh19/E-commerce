namespace SimpleShop.DTOs.WishlistDto
{
    public class WishlistDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();

    }
    public class AddWishlistRequestDto
    {
        public int ProductId { get; set; }
    }
}