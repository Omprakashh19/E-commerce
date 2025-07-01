namespace SimpleShop.DTOs
{
    public class ReturnRequestDto
    {
        public int OrderId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
