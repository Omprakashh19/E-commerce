using SimpleShop.DTOs.OrderDto;
using SimpleShop.Models.OrderModels;

namespace SimpleShop.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> PlaceOrderAsync(int userId, int shippingAddressId);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<List<Order>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(int orderId, string newStatus);
        Task<bool> SimulatePaymentAsync(PaymentUpdateDto dto);
        Task<int?> GetLatestShippingAddressIdAsync(int userId); // 🔁 Added
    }
}
