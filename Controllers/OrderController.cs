using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.DTOs.OrderDto;
using SimpleShop.Services.Interfaces;
using System.Security.Claims;

namespace SimpleShop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private int GetUserIdFromToken()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
                throw new UnauthorizedAccessException("Invalid or missing user ID in token.");
            return userId;
        }

        // ✅ POST: Place order from cart
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequestDto dto)
        {
            int userId = GetUserIdFromToken();
            var result = await _orderService.PlaceOrderAsync(userId, dto.ShippingAddressId);

            return Ok(new
            {
                status = true,
                message = "Order placed successfully.",
                order = result
            });
        }



        // ✅ GET: View my order history
        [HttpGet("GetMyOrders")]
        public async Task<IActionResult> GetMyOrders()
        {
            int userId = GetUserIdFromToken();
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // ✅ GET: Admin - View all orders
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // ✅ PUT: Admin - Update order status
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateOrderStatus/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromQuery] string status)
        {
            await _orderService.UpdateOrderStatusAsync(id, status);
            return Ok(new { status = true, message = "Order status updated." });
        }
        [Authorize]
        [HttpPost("PayOrders")]
        public async Task<IActionResult> SimulatePayment([FromBody] PaymentUpdateDto dto)
        {
            var result = await _orderService.SimulatePaymentAsync(dto);
            if (!result)
                return BadRequest(new { status = false, message = "Payment failed or order not found." });

            return Ok(new { status = true, message = "Payment marked as paid." });
        }


    }
}
