
using SimpleShop.DTOs.ProductDto;
using SimpleShop.Models.ProductsModels;

namespace SimpleShop.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<List<Product>> SearchAsync(string keyword);
        Task<(List<Product>, int)> SearchFilteredAsync(ProductSearchFilter filter);
        Task<List<Product>> GetLowStockProductsAsync(int threshold);

    }
}
