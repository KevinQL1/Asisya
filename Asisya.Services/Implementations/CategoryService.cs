using Asisya.Data.Persistence;
using Asisya.Entity;
using Asisya.Services.DTOs;
using Asisya.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Asisya.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Picture = c.Picture
            })
            .ToListAsync();
    }

    public async Task<int> CreateCategoryAsync(CategoryDto request)
    {
        var category = new Category
        {
            CategoryName = request.CategoryName,
            Description = request.Description,
            Picture = request.Picture
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category.CategoryId;
    }
}