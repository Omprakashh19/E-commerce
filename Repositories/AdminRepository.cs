using Microsoft.EntityFrameworkCore;
using SimpleShop.Data;
using SimpleShop.Repositories.Interfaces;

namespace SimpleShop.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.PaymentDetails
                .Where(p => p.Status == "Paid")
                .SumAsync(p => p.Amount);
        }
    }
}
