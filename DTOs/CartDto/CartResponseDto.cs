namespace SimpleShop.DTOs.CartDto
{
    public class CartResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public List<CartItemDto> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.SubTotal);
    }
}
