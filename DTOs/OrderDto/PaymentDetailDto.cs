namespace SimpleShop.DTOs.OrderDto
{
    public class PaymentDetailDto
{
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

}