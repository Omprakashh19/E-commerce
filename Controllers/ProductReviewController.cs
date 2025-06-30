using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.DTOs;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _reviewService;

        public ProductReviewController(IProductReviewService reviewService)
        {
            _reviewService = reviewService;

        }

        private int GetUserIdFromToken()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim == null) throw new Exception("UserId claim is missing in token");
            return int.Parse(claim.Value);

        }

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview([FromForm] AddReviewRequestDto dto)
        {
            int userId = GetUserIdFromToken();
            var result = await _reviewService.AddReviewAsync(userId, dto);


            if (!result)
            {
                return BadRequest(new { status = false, message = "You have already reviewed this product." });
            }

            return Ok(new { status = true, message = "Review added successfully." });

        }

        [HttpGet("GetReview/{productId}")]
        public async Task<IActionResult> GetReviews(int productId)
        {
            var reviews = await _reviewService.GetProductReviewsAsync(productId);
            if (reviews == null || !reviews.Any())
                return NotFound(new { status = false, message = "No reviews found for this product." });

            return Ok(new { status = true, message = "Reviews fetched successfully.", data = reviews });
        }

        [HttpPut("UpdateReview")]
        public async Task<IActionResult> UpdateReviews([FromForm] UpdateReviewRequestDto dto)
        {
            int userId = GetUserIdFromToken();
            var updated = await _reviewService.UpdateReviewAsync(userId, dto);
            if (!updated)
                return NotFound(new { status = false, message = "Review not found to update." });

            return Ok(new { status = true, message = "Review Updated Succesfully", data = updated });
        }
        [HttpDelete("DeleteReviews/{productId}")]
        public async Task<IActionResult> DeleteReview(int productId)
        {
            int userId = GetUserIdFromToken();
            var deleted = await _reviewService.DeleteReviewAsync(userId, productId);
            if (!deleted)
                return NotFound(new { status = false, message = "Review not found to delete." });
            return Ok(new { status = true, message = "Review Deleted Succesfully" });

        }

        [HttpDelete("image/{imageId}")]
        public async Task<IActionResult> DeleteReviewImage(int imageId)
        {
            var deleted = await _reviewService.DeleteReviewImageAsync(imageId);
            if (!deleted) return NotFound("Image not found.");

            return Ok("Image deleted successfully!");
        }

    }

}
