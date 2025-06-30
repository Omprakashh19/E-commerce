namespace SimpleShop.DTOs.OrderDto
{
    public class PaymentUpdateDto
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } = "COD";
    }
}
