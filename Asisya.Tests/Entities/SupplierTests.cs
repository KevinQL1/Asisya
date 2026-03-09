using Asisya.Entity;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Entities;

public class SupplierTests
{
    [Fact]
    public void Supplier_Should_Initialize_With_Default_Values()
    {
        var supplier = new Supplier();

        supplier.CompanyName.Should().Be(string.Empty);
        supplier.Products.Should().NotBeNull();
        supplier.Products.Should().BeEmpty();
    }

    [Fact]
    public void Supplier_Should_Set_Properties_Correctly()
    {
        var supplier = new Supplier
        {
            SupplierId = 1,
            CompanyName = "Exotic Liquids",
            ContactName = "Charlotte Cooper",
            ContactTitle = "Purchasing Manager",
            Address = "49 Gilbert St.",
            City = "London",
            PostalCode = "EC1 4SD",
            Country = "UK",
            Phone = "(171) 555-2222",
            HomePage = "exoticliquids.com"
        };

        supplier.SupplierId.Should().Be(1);
        supplier.CompanyName.Should().Be("Exotic Liquids");
        supplier.ContactName.Should().Be("Charlotte Cooper");
        supplier.ContactTitle.Should().Be("Purchasing Manager");
        supplier.Address.Should().Be("49 Gilbert St.");
        supplier.City.Should().Be("London");
        supplier.PostalCode.Should().Be("EC1 4SD");
        supplier.Country.Should().Be("UK");
        supplier.Phone.Should().Be("(171) 555-2222");
        supplier.HomePage.Should().Be("exoticliquids.com");
    }

    [Fact]
    public void Supplier_Should_Maintain_Products_Collection()
    {
        var supplier = new Supplier { SupplierId = 1, CompanyName = "Exotic Liquids" };
        var product = new Product { ProductId = 1, ProductName = "Chai", Supplier = supplier };

        supplier.Products.Add(product);

        supplier.Products.Should().HaveCount(1);
        supplier.Products.First().ProductName.Should().Be("Chai");
    }
}