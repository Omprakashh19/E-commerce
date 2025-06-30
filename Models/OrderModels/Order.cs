namespace SimpleShop.Models.OrderModels
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        public List<OrderItem> Items { get; set; } = new();

        public PaymentDetail PaymentDetail { get; set; }
        public ShippingAddress ShippingAddress { get; set; }




    }
}