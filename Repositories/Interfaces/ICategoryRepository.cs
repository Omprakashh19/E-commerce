using SimpleShop.Models.ProductsModels;

namespace SimpleShop.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task<bool> UpdateAsync(Category category);   // Update category details
        Task<bool> DeleteAsync(int id);              // Delete category by id
    }
}
