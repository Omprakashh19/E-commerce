using SimpleShop.DTOs;
using SimpleShop.DTOs.ProductDto;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetAllAsync();

    Task<ProductResponseDto?> GetByIdAsync(int id);
    Task<ProductResponseDto> AddProductAsync(ProductDto dto);
    Task<bool> UpdateProductAsync(int id, ProductDto dto);
    Task<bool> DeleteProductAsync(int id);
    Task<List<ProductResponseDto>> SearchAsync(string keyword);
    Task<PagedResult<ProductResponseDto>> SearchFilteredAsync(ProductSearchFilter filter);
    Task<List<ProductResponseDto>> GetLowStockProductsAsync(int threshold);

}
