using Microsoft.EntityFrameworkCore;
using SimpleShop.Data;
using SimpleShop.DTOs.OrderDto;
using SimpleShop.Models.OrderModels;
using SimpleShop.Repositories.Interfaces;

namespace SimpleShop.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> PlaceOrderAsync(int userId, int shippingAddressId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
                throw new Exception("Cart is empty.");

            var userAddress = await _context.UserShippingAddresses
                .FirstOrDefaultAsync(a => a.Id == shippingAddressId && a.UserId == userId);

            if (userAddress == null)
                throw new Exception("Invalid shipping address.");


            // ✅ Check & update stock before creating order
            foreach (var item in cart.Items)
            {
                var product = item.Product!;
                if (product.StockQuantity < item.Quantity)
                    throw new Exception($"Insufficient stock for product: {product.Name}");

                product.StockQuantity -= item.Quantity;

                if (product.StockQuantity < 5)
                {
                    Console.WriteLine($"⚠️ Low stock alert: {product.Name} has only {product.StockQuantity} left.");
                }
            }
            var order = new Order
            {
                UserId = userId,
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product!.Name,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList(),

                PaymentDetail = new PaymentDetail
                {
                    PaymentMethod = "COD",
                    Status = "Paid",
                    Amount = cart.Items.Sum(i => i.Product!.Price * i.Quantity)
                },

                ShippingAddress = new ShippingAddress
                {
                    FullName = userAddress.FullName,
                    AddressLine = userAddress.AddressLine,
                    City = userAddress.City,
                    State = userAddress.State,
                    ZipCode = userAddress.ZipCode
                }
            };

            _context.Orders.Add(order);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<int?> GetLatestShippingAddressIdAsync(int userId)
        {
            var address = await _context.UserShippingAddresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();

            return address?.Id;

        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.PaymentDetail)
                .Include(o => o.ShippingAddress)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.PaymentDetail)
                .Include(o => o.ShippingAddress)
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new Exception("Order not found.");

            order.Status = newStatus;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SimulatePaymentAsync(PaymentUpdateDto dto)
        {
            var order = await _context.Orders
                .Include(o => o.PaymentDetail)
                .FirstOrDefaultAsync(o => o.Id == dto.OrderId);

            if (order == null || order.PaymentDetail == null)
                return false;

            order.PaymentDetail.PaymentMethod = dto.PaymentMethod;
            order.PaymentDetail.Status = "Paid";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }

        // ✅ NEW: Implement UpdateAsync
        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }

}
