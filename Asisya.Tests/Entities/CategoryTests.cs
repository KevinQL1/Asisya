using Asisya.Entity;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Entities;

public class CategoryTests
{
    [Fact]
    public void Category_Should_Initialize_With_Default_Values()
    {
        var category = new Category();

        category.CategoryName.Should().Be(string.Empty);
        category.Products.Should().NotBeNull();
        category.Products.Should().BeEmpty();
    }

    [Fact]
    public void Category_Should_Set_Properties_Correctly()
    {
        var category = new Category
        {
            CategoryId = 1,
            CategoryName = "SERVIDORES",
            Description = "Infraestructura física",
            Picture = "base64_string"
        };

        category.CategoryId.Should().Be(1);
        category.CategoryName.Should().Be("SERVIDORES");
        category.Description.Should().Be("Infraestructura física");
        category.Picture.Should().Be("base64_string");
    }

    [Fact]
    public void Category_Should_Allow_Adding_Products()
    {
        var category = new Category { CategoryName = "CLOUD" };
        var product = new Product { ProductName = "Azure Instance", Category = category };

        category.Products.Add(product);

        category.Products.Should().HaveCount(1);
        category.Products.First().ProductName.Should().Be("Azure Instance");
    }
}