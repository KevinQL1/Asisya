using Asisya.Data.Persistence;
using Asisya.Entity;
using Asisya.Services.DTOs;
using Asisya.Services.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Asisya.Services.Implementations;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context) => _context = context;

    public async Task<int> CreateProductAsync(ProductCreateDto dto)
    {
        var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId);
        if (!categoryExists)
        {
            throw new KeyNotFoundException($"La categoría con ID {dto.CategoryId} no existe.");
        }

        var product = new Product
        {
            ProductName = dto.ProductName,
            CategoryId = dto.CategoryId,
            UnitPrice = dto.UnitPrice,
            UnitsInStock = dto.UnitsInStock,
            Discontinued = false
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product.ProductId;
    }

    public async Task GenerateMassiveProductsAsync(int count)
    {
        if (count < 100 || count > 100000)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "La cantidad debe estar entre 100 y 100,000.");
        }

        var random = new Random();
        var products = new List<Product>();
        var categoryIds = await _context.Categories.Select(c => c.CategoryId).ToListAsync();

        if (categoryIds.Count == 0)
        {
            throw new InvalidOperationException("No existen categorías para asociar los productos.");
        }

        for (int i = 0; i < count; i++)
        {
            products.Add(new Product
            {
                ProductName = $"Product_{Guid.NewGuid().ToString().Substring(0, 8)}",
                CategoryId = categoryIds[random.Next(categoryIds.Count)],
                UnitPrice = (decimal)(random.NextDouble() * 5000),
                UnitsInStock = (short)random.Next(1, 1000),
                Discontinued = false
            });
        }

        await _context.BulkInsertAsync(products);
    }

    public async Task<PagedResult<ProductResponseDto>> GetProductsAsync(int page, int pageSize, string? search, int? categoryId)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.ProductName.ToLower().Contains(search.ToLower()));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        var totalRecords = await query.CountAsync();

        var products = await query
            .OrderBy(p => p.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductResponseDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock,
                CategoryName = p.Category != null ? p.Category.CategoryName : "N/A",
                CategoryPicture = p.Category != null ? p.Category.Picture : null
            }).ToListAsync();

        return new PagedResult<ProductResponseDto>
        {
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
            CurrentPage = page,
            Data = products
        };
    }

    public async Task<ProductDetailDto?> GetProductByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductId == id);

        if (product == null) return null;

        return new ProductDetailDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            UnitPrice = product.UnitPrice,
            UnitsInStock = product.UnitsInStock,
            QuantityPerUnit = product.QuantityPerUnit,
            Discontinued = product.Discontinued,
            CategoryName = product.Category?.CategoryName ?? "N/A",
            CategoryPicture = product.Category?.Picture
        };
    }

    public async Task<ProductDetailDto?> UpdateProductAsync(int id, ProductUpdateDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return null;

        var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId);
        if (!categoryExists)
        {
            throw new KeyNotFoundException($"La categoría con ID {dto.CategoryId} no existe.");
        }

        product.ProductName = dto.ProductName;
        product.CategoryId = dto.CategoryId;
        product.UnitPrice = dto.UnitPrice;
        product.UnitsInStock = dto.UnitsInStock;

        await _context.SaveChangesAsync();

        return await GetProductByIdAsync(id);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}