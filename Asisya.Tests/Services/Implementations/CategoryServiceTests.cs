using Asisya.Data.Persistence;
using Asisya.Entity;
using Asisya.Services.DTOs;
using Asisya.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DotNetEnv; //

namespace Asisya.Tests.Services.Implementations;

public class CategoryServiceTests
{
    public CategoryServiceTests()
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
    public async Task CreateCategoryAsync_Should_Save_Category_And_Return_Id()
    {
        using var context = GetDbContext();
        var service = new CategoryService(context);
        var dto = new CategoryDto
        {
            CategoryName = "SERVIDORES",
            Description = "Infraestructura de red",
            Picture = "base64_data"
        };

        var resultId = await service.CreateCategoryAsync(dto);

        resultId.Should().BeGreaterThan(0);
        
        var categoryInDb = await context.Categories.FindAsync(resultId);
        categoryInDb.Should().NotBeNull();
        categoryInDb!.CategoryName.Should().Be("SERVIDORES");
        categoryInDb.Description.Should().Be("Infraestructura de red");
        categoryInDb.Picture.Should().Be("base64_data");
    }

    [Fact]
    public async Task CreateCategoryAsync_Should_Increment_Id_For_Multiple_Categories()
    {
        using var context = GetDbContext();
        var service = new CategoryService(context);
        var cat1 = new CategoryDto { CategoryName = "CLOUD" };
        var cat2 = new CategoryDto { CategoryName = "HARDWARE" };

        var id1 = await service.CreateCategoryAsync(cat1);
        var id2 = await service.CreateCategoryAsync(cat2);

        id2.Should().NotBe(id1);
        (await context.Categories.CountAsync()).Should().Be(2);
    }
}