using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SimpleShop.Models.CartModels;
using SimpleShop.Models.ProductsModels; // Add this namespace

public class CartItem
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("CartId")]
    public int CartId { get; set; }

    [JsonIgnore]
    public Cart? Cart { get; set; }

    public Product? Product { get; set; }
}
