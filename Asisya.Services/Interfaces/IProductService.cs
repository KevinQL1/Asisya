using Asisya.Services.DTOs;

namespace Asisya.Services.Interfaces;

public interface IProductService
{
    Task<int> CreateProductAsync(ProductCreateDto dto);

    Task GenerateMassiveProductsAsync(int count);
    
    Task<PagedResult<ProductResponseDto>> GetProductsAsync(int page, int pageSize, string? search, int? categoryId);
    
    Task<ProductDetailDto?> GetProductByIdAsync(int id);
    
    Task<ProductDetailDto?> UpdateProductAsync(int id, ProductUpdateDto dto);    
    Task<bool> DeleteProductAsync(int id);
}