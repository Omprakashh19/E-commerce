using Microsoft.EntityFrameworkCore;
using SimpleShop.Data;
using SimpleShop.DTOs.ProductDto;
using SimpleShop.Models.ProductsModels;
using SimpleShop.Repositories.Interfaces;

namespace SimpleShop.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var result = await (from p in _context.Products
                                join c in _context.Categories
                                on p.CategoryId equals c.Id
                                select new Product
                                {

                                    Id = p.Id,
                                    Name = p.Name,
                                    Description = p.Description,
                                    Price = p.Price,
                                    StockQuantity = p.StockQuantity,
                                    Category = c,
                                    Images = _context.ProductImages
                                                            .Where(i => i.ProductId == p.Id)
                                                            .ToList(),
                                    AverageRating = p.AverageRating,
                                    ReviewCount = p.ReviewCount


                                }).ToListAsync();
            return result;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var result = await (from p in _context.Products
                                join c in _context.Categories on p.CategoryId equals c.Id
                                where p.Id == id
                                select new Product
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Description = p.Description,
                                    Price = p.Price,
                                    StockQuantity = p.StockQuantity,
                                    Category = c,
                                    Images = _context.ProductImages
                                                .Where(i => i.ProductId == p.Id)
                                                .ToList(),
                                    AverageRating = p.AverageRating,
                                    ReviewCount = p.ReviewCount
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> SearchAsync(string keyword)
        {
            var result = await (from p in _context.Products
                                join c in _context.Categories on p.CategoryId equals c.Id
                                where p.Name.Contains(keyword) || p.Description.Contains(keyword)
                                select new Product
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Description = p.Description,
                                    Price = p.Price,
                                    StockQuantity = p.StockQuantity,
                                    Category = c,
                                    Images = _context.ProductImages
                                                .Where(i => i.ProductId == p.Id)
                                                .ToList(),
                                    AverageRating = p.AverageRating,
                                    ReviewCount = p.ReviewCount
                                }).ToListAsync();

            return result;
        }


        public async Task<(List<Product>, int)> SearchFilteredAsync(ProductSearchFilter filter)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Keyword))
                query = query.Where(p => p.Name.Contains(filter.Keyword) || p.Description.Contains(filter.Keyword));

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice);

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId);

            if (filter.MinRating.HasValue)
                query = query.Where(p => p.AverageRating >= filter.MinRating);

            // Sorting
            query = filter.SortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "rating" => query.OrderByDescending(p => p.AverageRating),
                _ => query.OrderBy(p => p.Name)
            };

            var total = await query.CountAsync();

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<List<Product>> GetLowStockProductsAsync(int threshold)
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(p => p.StockQuantity < threshold)
                .ToListAsync();
        }


    }
}
