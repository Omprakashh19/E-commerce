using SimpleShop.Models;

namespace SimpleShop.Repositories.Interfaces
{
    public interface IReturnRepository
    {
        Task AddAsync(ReturnRequest request);
        Task<ReturnRequest?> GetByIdAsync(int id);
        Task<List<ReturnRequest>> GetAllAsync();
        Task UpdateAsync(ReturnRequest request);
    }

}


