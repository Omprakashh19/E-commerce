using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.Services.Interfaces;
using System.Security.Claims;

namespace SimpleShop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // ✅ Extracts userId from token claims
        private int GetUserIdFromToken()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
                throw new UnauthorizedAccessException("Invalid or missing user ID in token.");
            return userId;
        }

        [HttpGet("GetMyCart")]
        public async Task<IActionResult> GetCart()
        {
            int userId = GetUserIdFromToken();
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("AddCart")]
        public async Task<IActionResult> AddItem([FromQuery] int productId, [FromQuery] int quantity)
        {
            int userId = GetUserIdFromToken();
            await _cartService.AddItemAsync(userId, productId, quantity);
            return Ok(new { status = true, message = "Item added" });
        }

        [HttpDelete("RemoveCart")]
        public async Task<IActionResult> RemoveItem([FromQuery] int productId)
        {
            int userId = GetUserIdFromToken();
            await _cartService.RemoveItemAsync(userId, productId);
            return Ok(new { status = true, message = "Item removed" });
        }

        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateQuantity([FromQuery] int productId, [FromQuery] int quantity)
        {
            int userId = GetUserIdFromToken();
            await _cartService.UpdateQuantityAsync(userId, productId, quantity);
            return Ok(new { status = true, message = "Quantity updated" });
        }
    }
}
