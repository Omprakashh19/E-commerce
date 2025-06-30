using SimpleShop.DTOs.OrderDto;
using SimpleShop.Models.OrderModels;
using SimpleShop.Repositories.Interfaces;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> PlaceOrderAsync(int userId)
        {
            var shippingAddressId = await _orderRepository.GetLatestShippingAddressIdAsync(userId);
            if (shippingAddressId == null)
                throw new Exception("No shipping address found for user.");

            return await PlaceOrderAsync(userId, shippingAddressId.Value);
        }

        public async Task<OrderDto> PlaceOrderAsync(int userId, int shippingAddressId)
        {
            var order = await _orderRepository.PlaceOrderAsync(userId, shippingAddressId);
            return MapToOrderDto(order);
        }

        public async Task<List<OrderDto>> GetOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapToOrderDto).ToList();
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            return await GetOrdersAsync(userId);
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return orders.Select(MapToOrderDto).ToList();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus);
        }

        public async Task<bool> SimulatePaymentAsync(PaymentUpdateDto dto)
        {
            return await _orderRepository.SimulatePaymentAsync(dto);
        }

        private OrderDto MapToOrderDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                PaymentDetail = new PaymentDetailDto
                {
                    PaymentMethod = order.PaymentDetail.PaymentMethod,
                    Status = order.PaymentDetail.Status,
                    Amount = order.PaymentDetail.Amount
                },
                ShippingAddress = new ShippingAddressDto
                {
                    FullName = order.ShippingAddress.FullName,
                    AddressLine = order.ShippingAddress.AddressLine,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    ZipCode = order.ShippingAddress.ZipCode
                }
            };
        }
    }
}
