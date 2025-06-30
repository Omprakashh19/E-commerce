using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.DTOs;
using SimpleShop.DTOs.ProductDto;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddProducts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm] ProductDto dto)
        {
            try
            {
                var product = await _productService.AddProductAsync(dto);
                return Ok(new { status = true, message = "Product added successfully", data = product });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateProduct/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductDto dto)
        {
            var updated = await _productService.UpdateProductAsync(id, dto);
            if (!updated)
            {
                return NotFound(new { status = false, message = "Product not found" });
            }

            return Ok(new { status = true, message = "Product updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteProduct/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
            {
                return NotFound(new { status = false, message = "Product not found" });
            }

            return Ok(new { status = true, message = "Product deleted successfully" });
        }

        [AllowAnonymous]
        [HttpGet("GetProductsById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound(new { status = false, message = "Product not found" });
            return Ok(product);
        }


        [HttpGet("SearchProducts")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var results = await _productService.SearchAsync(keyword);
            return Ok(new { status = true, data = results });
        }

        [HttpPost("FilterProducts")]
        public async Task<IActionResult> SearchProducts([FromBody] ProductSearchFilter filter)
        {
            var result = await _productService.SearchFilteredAsync(filter);
            return Ok(new { status = true, data = result });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("lowstock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold = 5)
        {
            var lowStockProducts = await _productService.GetLowStockProductsAsync(threshold);
            return Ok(lowStockProducts);
        }

    }
}