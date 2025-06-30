using SimpleShop.DTOs.OrderDto;

namespace SimpleShop.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> PlaceOrderAsync(int userId);
        Task<List<OrderDto>> GetOrdersAsync(int userId);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task UpdateOrderStatusAsync(int orderId, string newStatus);
        Task<OrderDto> PlaceOrderAsync(int userId, int shippingAddressId);
        Task<bool> SimulatePaymentAsync(PaymentUpdateDto dto);

    }
}
