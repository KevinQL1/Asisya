using Asisya.Services.DTOs;

namespace Asisya.Services.Interfaces;

public interface ICategoryService
{
    Task<int> CreateCategoryAsync(CategoryDto request);
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
}