using Asisya.Entity;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void Product_Should_Initialize_With_Default_Values()
    {
        var product = new Product();

        product.ProductName.Should().Be(string.Empty);
        product.OrderDetails.Should().NotBeNull();
        product.OrderDetails.Should().BeEmpty();
        product.Discontinued.Should().BeFalse();
    }

    [Fact]
    public void Product_Should_Set_Properties_Correctly()
    {
        var product = new Product
        {
            ProductId = 1,
            ProductName = "Chai",
            SupplierId = 1,
            CategoryId = 1,
            QuantityPerUnit = "10 boxes x 20 bags",
            UnitPrice = 18.00m,
            UnitsInStock = 39,
            Discontinued = false
        };

        product.ProductId.Should().Be(1);
        product.ProductName.Should().Be("Chai");
        product.UnitPrice.Should().Be(18.00m);
        product.UnitsInStock.Should().Be(39);
        product.CategoryId.Should().Be(1);
    }

    [Fact]
    public void Product_Should_Link_To_Category_And_Supplier()
    {
        var category = new Category { CategoryId = 2, CategoryName = "Condiments" };
        var supplier = new Supplier { SupplierId = 1, CompanyName = "Exotic Liquids" };

        var product = new Product
        {
            Category = category,
            Supplier = supplier
        };

        product.Category.Should().NotBeNull();
        product.Supplier.Should().NotBeNull();
        product.Category!.CategoryName.Should().Be("Condiments");
        product.Supplier!.CompanyName.Should().Be("Exotic Liquids");
    }

    [Fact]
    public void Product_Should_Maintain_OrderDetails_Collection()
    {
        var product = new Product { ProductId = 1, ProductName = "Test Product" };
        var detail = new OrderDetail { ProductId = 1, OrderId = 101, Quantity = 5 };

        product.OrderDetails.Add(detail);

        product.OrderDetails.Should().HaveCount(1);
        product.OrderDetails.First().OrderId.Should().Be(101);
    }
}