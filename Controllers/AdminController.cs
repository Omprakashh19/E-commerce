using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalUsers = await _adminService.GetTotalUsersAsync();
            var totalOrders = await _adminService.GetTotalOrdersAsync();
            var totalRevenue = await _adminService.GetTotalRevenueAsync();

            return Ok(new
            {
                totalUsers,
                totalOrders,
                totalRevenue
            });
        }

        
    }
}
