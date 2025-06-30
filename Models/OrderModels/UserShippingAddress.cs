using SimpleShop.Models;

public class UserShippingAddress
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string AddressLine { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public bool IsDefault { get; set; } = false;
}
