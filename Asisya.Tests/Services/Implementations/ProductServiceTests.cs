using Asisya.Data.Persistence;
using Asisya.Entity;
using Asisya.Services.DTOs;
using Asisya.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DotNetEnv;

namespace Asisya.Tests.Services.Implementations;

public class ProductServiceTests
{
    public ProductServiceTests()
    {
        var projectDir = Directory.GetCurrentDirectory();
        var envPath = Path.Combine(projectDir, "..", "..", "..", "..", ".env");
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
        }
    }

    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CreateProductAsync_Should_ThrowException_When_CategoryDoesNotExist()
    {
        using var context = GetDbContext();
        var service = new ProductService(context);
        var dto = new ProductCreateDto { CategoryId = 99, ProductName = "Fail" };

        Func<Task> act = async () => await service.CreateProductAsync(dto);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*La categoría con ID 99 no existe.*");
    }

    [Fact]
    public async Task CreateProductAsync_Should_Save_Product_When_CategoryExists()
    {
        using var context = GetDbContext();
        context.Categories.Add(new Category { CategoryId = 1, CategoryName = "Test" });
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var dto = new ProductCreateDto 
        { 
            ProductName = "New Product", 
            CategoryId = 1, 
            UnitPrice = 100, 
            UnitsInStock = 10 
        };

        var resultId = await service.CreateProductAsync(dto);

        resultId.Should().BeGreaterThan(0);
        (await context.Products.CountAsync()).Should().Be(1);
    }

    // Se hace un Skip porque EFCore.BulkExtensions requiere PostgreSQL real, no funciona en InMemory
    [Fact(Skip = "EFCore.BulkExtensions requiere un proveedor de base de datos relacional real (PostgreSQL).")]
    public async Task GenerateMassiveProductsAsync_Should_Create_Requested_Count()
    {
        using var context = GetDbContext();
        context.Categories.Add(new Category { CategoryId = 1, CategoryName = "Cloud" });
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        int requestedCount = 50;

        await service.GenerateMassiveProductsAsync(requestedCount);

        var total = await context.Products.CountAsync();
        total.Should().Be(requestedCount);
    }

    [Fact]
    public async Task GetProductsAsync_Should_Return_Paged_And_Filtered_Results()
    {
        using var context = GetDbContext();
        var cat = new Category { CategoryId = 1, CategoryName = "Hardware" };
        context.Categories.Add(cat);
        context.Products.AddRange(new List<Product>
        {
            new Product { ProductName = "Mouse", CategoryId = 1 },
            new Product { ProductName = "Keyboard", CategoryId = 1 },
            new Product { ProductName = "Monitor", CategoryId = 1 }
        });
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var result = await service.GetProductsAsync(1, 2, "key", null);

        result.TotalRecords.Should().Be(1);
        result.Data.Should().HaveCount(1);
        result.Data.First().ProductName.Should().Be("Keyboard");
    }

    [Fact]
    public async Task UpdateProductAsync_Should_Modify_Existing_Product()
    {
        using var context = GetDbContext();
        context.Categories.Add(new Category { CategoryId = 1, CategoryName = "Old" });
        context.Categories.Add(new Category { CategoryId = 2, CategoryName = "New" });
        var product = new Product { ProductId = 1, ProductName = "Old Name", CategoryId = 1 };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var updateDto = new ProductUpdateDto 
        { 
            ProductName = "Updated Name", 
            CategoryId = 2, 
            UnitPrice = 50, 
            UnitsInStock = 5 
        };

        var result = await service.UpdateProductAsync(1, updateDto);

        result.Should().NotBeNull();
        result!.ProductName.Should().Be("Updated Name");
        result.CategoryName.Should().Be("New");
    }

    [Fact]
    public async Task DeleteProductAsync_Should_Remove_Product_From_Db()
    {
        using var context = GetDbContext();
        var product = new Product { ProductId = 1, ProductName = "To Delete" };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var success = await service.DeleteProductAsync(1);
        var exists = await context.Products.AnyAsync(p => p.ProductId == 1);

        success.Should().BeTrue();
        exists.Should().BeFalse();
    }
}