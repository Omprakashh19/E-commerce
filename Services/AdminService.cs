using SimpleShop.Repositories.Interfaces;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;

        public AdminService(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public Task<int> GetTotalUsersAsync() => _adminRepo.GetTotalUsersAsync();
        public Task<int> GetTotalOrdersAsync() => _adminRepo.GetTotalOrdersAsync();
        public Task<decimal> GetTotalRevenueAsync() => _adminRepo.GetTotalRevenueAsync();
    }
}
