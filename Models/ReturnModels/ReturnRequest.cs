using SimpleShop.Models.OrderModels;

namespace SimpleShop.Models
{
    public class ReturnRequest
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public Order Order { get; set; }  
    }
}
