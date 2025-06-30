namespace SimpleShop.DTOs.OrderDto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();

        public PaymentDetailDto PaymentDetail { get; set; }  // ✅ Add this
        public ShippingAddressDto ShippingAddress { get; set; }
    }

}
