using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.DTOs.WishlistDto;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        private int GetUserIdFromToken()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new Exception("UserId claim is missing in the token.");

            return int.Parse(claim.Value);
        }



        [HttpPost("AddWishlist")]
        public async Task<IActionResult> AddWishlist([FromBody] AddWishlistRequestDto dto)
        {
            int userId = GetUserIdFromToken();
            var result = await _wishlistService.AddToWishlistAsync(userId, dto.ProductId);

            if (!result)
            {
                return BadRequest(new { status = false, message = "Product already in wishlist." });
            }

            return Ok(new { status = true, message = "Product added to wishlist." });
        }

        [HttpGet("AllMyWishlist")]
        public async Task<IActionResult> GetUserWishlist()
        {
            int userId = GetUserIdFromToken();
            var result = await _wishlistService.GetUserWishListAsync(userId);

            if (result == null || !result.Any())
            {
                return NotFound(new { status = false, message = "No wishlist items found." });
            }

            return Ok(new { status = true, message = "Wishlist fetched successfully.", data = result });
        }

        [HttpDelete("RemoveWishlist/{productId}")]
        public async Task<IActionResult> RemoveWishlist(int productId)
        {
            int userId = GetUserIdFromToken();
            var removed = await _wishlistService.RemoveFromWishlistAsync(userId, productId);

            if (!removed)
            {
                return NotFound(new { status = false, message = "Product not found in wishlist." });
            }

            return Ok(new { status = true, message = "Product removed from wishlist." });
        }
    }
}
