using Asisya.Services.DTOs;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Services.DTOs;

public class ProductDtoTests
{
    [Fact]
    public void ProductCreateDto_Should_Set_Properties_Correctly()
    {
        var dto = new ProductCreateDto
        {
            ProductName = "Laptop",
            CategoryId = 1,
            UnitPrice = 1500.50m,
            UnitsInStock = 10
        };

        dto.ProductName.Should().Be("Laptop");
        dto.CategoryId.Should().Be(1);
        dto.UnitPrice.Should().Be(1500.50m);
        dto.UnitsInStock.Should().Be(10);
    }

    [Fact]
    public void ProductDetailDto_Should_Inherit_And_Include_CategoryPicture()
    {
        var dto = new ProductDetailDto
        {
            ProductId = 1,
            ProductName = "Smartphone",
            UnitPrice = 800m,
            CategoryName = "Electronics",
            CategoryPicture = "base64_data",
            Discontinued = false
        };

        dto.ProductId.Should().Be(1);
        dto.CategoryName.Should().Be("Electronics");
        dto.CategoryPicture.Should().Be("base64_data");
        dto.Discontinued.Should().BeFalse();
    }

    [Fact]
    public void PagedResult_Should_Initialize_Correctly()
    {
        var data = new List<string> { "Item1", "Item2" };
        var pagedResult = new PagedResult<string>
        {
            TotalRecords = 100,
            TotalPages = 10,
            CurrentPage = 1,
            Data = data
        };

        pagedResult.TotalRecords.Should().Be(100);
        pagedResult.Data.Should().HaveCount(2);
        pagedResult.Data.Should().Contain("Item1");
    }
}