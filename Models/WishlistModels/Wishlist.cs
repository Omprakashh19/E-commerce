using SimpleShop.Models.ProductsModels;

namespace SimpleShop.Models.WishListModels
{
    public class WishList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.Now;

    }
}