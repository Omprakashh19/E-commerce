using System.Threading.Tasks;

namespace SimpleShop.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<int> GetTotalUsersAsync();
        Task<int> GetTotalOrdersAsync();
        Task<decimal> GetTotalRevenueAsync();
    }
}
