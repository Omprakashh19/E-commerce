namespace SimpleShop.Models.OrderModels
{
    public class ShippingAddress
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
