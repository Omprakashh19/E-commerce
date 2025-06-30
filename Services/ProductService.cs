using SimpleShop.DTOs;
using SimpleShop.DTOs.ProductDto;
using SimpleShop.Models.ProductsModels;
using SimpleShop.Repositories.Interfaces;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ProductResponseDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapProductToResponse).ToList();
    }
    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;

        return MapProductToResponse(product);
    }


    public async Task<ProductResponseDto> AddProductAsync(ProductDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (category == null)
            throw new Exception("Invalid category");

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            Images = new List<ProductImage>(),
            StockQuantity = dto.StockQuantity,
            DiscountPercentage = dto.DiscountPercentage,
            DiscountAmount = dto.DiscountAmount,


        };
        foreach (var file in dto.Images)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                product.Images.Add(new ProductImage { ImageUrl = $"/images/{uniqueFileName}" });
            }
        }

        await _productRepository.AddAsync(product);
        product.Category = category;

        return MapProductToResponse(product);
    }

    public async Task<bool> UpdateProductAsync(int id, ProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;
        product.Images = new List<ProductImage>();
        product.StockQuantity = dto.StockQuantity;
        product.DiscountPercentage = dto.DiscountPercentage;
        product.DiscountAmount = dto.DiscountAmount;

        foreach (var file in dto.Images)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                product.Images.Add(new ProductImage { ImageUrl = $"/images/{uniqueFileName}" });
            }
        }

        await _productRepository.UpdateAsync(product);
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        await _productRepository.DeleteAsync(id);
        return true;
    }

    public async Task<List<ProductResponseDto>> SearchAsync(string keyword)
    {
        var products = await _productRepository.SearchAsync(keyword);
        return products.Select(MapProductToResponse).ToList();
    }

    public async Task<PagedResult<ProductResponseDto>> SearchFilteredAsync(ProductSearchFilter filter)
    {
        var (products, totalCount) = await _productRepository.SearchFilteredAsync(filter);

        var dtos = products.Select(MapProductToResponse).ToList();

        return new PagedResult<ProductResponseDto>
        {
            CurrentPage = filter.Page,
            PageSize = filter.PageSize,
            TotalCount = totalCount,
            Items = dtos
        };
    }


    public async Task<List<ProductResponseDto>> GetLowStockProductsAsync(int threshold)
    {
        var products = await _productRepository.GetLowStockProductsAsync(threshold);
        return products.Select(MapProductToResponse).ToList();
    }

    private ProductResponseDto MapProductToResponse(Product p)
    {

        decimal finalPrice = p.Price;

        if (p.DiscountPercentage.HasValue && p.DiscountPercentage > 0)
        {
            finalPrice -= p.Price * (p.DiscountPercentage.Value / 100);
        }
        else if (p.DiscountAmount.HasValue && p.DiscountAmount > 0)
        {
            finalPrice -= p.DiscountAmount.Value;
        }


        if (finalPrice < 0) finalPrice = 0;

        return new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            FinalPrice = finalPrice,
            DiscountPercentage = p.DiscountPercentage,
            DiscountAmount = p.DiscountAmount,
            CategoryName = p.Category.Name,
            ImageUrls = p.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>(),
            StockQuantity = p.StockQuantity,
            AverageRating = p.AverageRating,
            ReviewCount = p.ReviewCount
        };
    }

}
