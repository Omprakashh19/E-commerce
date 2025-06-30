using Microsoft.EntityFrameworkCore;
using SimpleShop.Models;
using SimpleShop.Models.CartModels;
using SimpleShop.Models.OrderModels;
using SimpleShop.Models.ProductReviewImagesModels;
using SimpleShop.Models.ProductReviewModels;
using SimpleShop.Models.ProductsModels;
using SimpleShop.Models.WishListModels;

namespace SimpleShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<UserShippingAddress> UserShippingAddresses { get; set; }
        public DbSet<WishList> wishlists { get; set; }
        public DbSet<ProductReview> productReviews { get; set; }
        public DbSet<ProductReviewImages> productReviewImages { get; set; } = null!;


    }
}
