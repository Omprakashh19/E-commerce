namespace SimpleShop.Models.OrderModels
{
    public class PaymentDetail
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; } = "COD"; // Cash, Card, UPI, etc.
        public string Status { get; set; } = "Paid"; // Paid, Failed, Pending
        public decimal Amount { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
