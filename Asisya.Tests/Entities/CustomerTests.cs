using Asisya.Entity;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Entities;

public class CustomerTests
{
    [Fact]
    public void Customer_Should_Initialize_With_Default_Values()
    {
        var customer = new Customer();

        customer.CustomerId.Should().Be(string.Empty);
        customer.CompanyName.Should().Be(string.Empty);
        customer.Orders.Should().NotBeNull();
        customer.Orders.Should().BeEmpty();
    }

    [Fact]
    public void Customer_Should_Set_Properties_Correctly()
    {
        var customer = new Customer
        {
            CustomerId = "ALFKI",
            CompanyName = "Alfreds Futterkiste",
            ContactName = "Maria Anders",
            City = "Berlin",
            Country = "Germany"
        };

        customer.CustomerId.Should().Be("ALFKI");
        customer.CompanyName.Should().Be("Alfreds Futterkiste");
        customer.ContactName.Should().Be("Maria Anders");
        customer.City.Should().Be("Berlin");
        customer.Country.Should().Be("Germany");
    }

    [Fact]
    public void Customer_Should_Maintain_Orders_Collection()
    {
        var customer = new Customer { CustomerId = "BOLID", CompanyName = "Bólido Comidas preparadas" };
        var order = new Order { OrderId = 1, Customer = customer };

        customer.Orders.Add(order);

        customer.Orders.Should().HaveCount(1);
        customer.Orders.First().OrderId.Should().Be(1);
    }
}