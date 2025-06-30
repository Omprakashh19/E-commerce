using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.DTOs.OrderDto;
using SimpleShop.Repositories.Interfaces;
using System.Security.Claims;

namespace SimpleShop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IUserAddressRepository _addressRepository;

        public ShippingAddressController(IUserAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        // ✅ POST: Add address
        [HttpPost("AddShippingAddress")]
        public async Task<IActionResult> AddAddress([FromBody] ShippingAddressDto dto, [FromQuery] bool isDefault = false)
        {
            int userId = GetUserId();
            await _addressRepository.AddAddressAsync(userId, dto, isDefault);
            return Ok(new { message = "Address added successfully." });
        }

        // ✅ GET: All addresses
        [HttpGet("GetAllUserShippingAddress")]
        public async Task<IActionResult> GetAddresses()
        {
            int userId = GetUserId();
            var addresses = await _addressRepository.GetAddressesAsync(userId);
            return Ok(addresses);
        }

        // ✅ PUT: Set default address
        [HttpPut("SetShippingAddress/{addressId}")]
        public async Task<IActionResult> SetDefault(int addressId)
        {
            int userId = GetUserId();
            await _addressRepository.SetDefaultAddressAsync(userId, addressId);
            return Ok(new { message = "Default address set." });
        }

        // ✅ GET: Default address only
        [HttpGet("GetSetShippingAddress")]
        public async Task<IActionResult> GetDefault()
        {
            int userId = GetUserId();
            var address = await _addressRepository.GetDefaultAddressAsync(userId);
            return address == null
                ? NotFound(new { message = "No default address found." })
                : Ok(address);
        }
    }
}
