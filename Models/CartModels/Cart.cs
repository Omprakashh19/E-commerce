using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleShop.Models.CartModels
{

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; } // ✅ Must match User.Id type
    public List<CartItem> Items { get; set; } = new();
}


}