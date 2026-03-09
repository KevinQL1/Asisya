using Asisya.Entity;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Entities;

public class OrderTests
{
    [Fact]
    public void Order_Should_Initialize_With_Default_Values()
    {
        var order = new Order();

        order.OrderDetails.Should().NotBeNull();
        order.OrderDetails.Should().BeEmpty();
    }

    [Fact]
    public void Order_Should_Set_Properties_Correctly()
    {
        var orderDate = DateTime.UtcNow;
        var order = new Order
        {
            OrderId = 10248,
            CustomerId = "VINET",
            EmployeeId = 5,
            OrderDate = orderDate,
            Freight = 32.38m,
            ShipName = "Vins et alcools Chevalier",
            ShipCountry = "France"
        };

        order.OrderId.Should().Be(10248);
        order.CustomerId.Should().Be("VINET");
        order.EmployeeId.Should().Be(5);
        order.OrderDate.Should().Be(orderDate);
        order.Freight.Should().Be(32.38m);
        order.ShipName.Should().Be("Vins et alcools Chevalier");
        order.ShipCountry.Should().Be("France");
    }

    [Fact]
    public void Order_Should_Allow_Adding_OrderDetails()
    {
        var order = new Order { OrderId = 1 };
        var detail = new OrderDetail { OrderId = 1, ProductId = 11, UnitPrice = 14.0m };

        order.OrderDetails.Add(detail);

        order.OrderDetails.Should().HaveCount(1);
        order.OrderDetails.First().ProductId.Should().Be(11);
    }

    [Fact]
    public void Order_Should_Link_To_Related_Entities()
    {
        var customer = new Customer { CustomerId = "ALFKI", CompanyName = "Alfreds" };
        var employee = new Employee { EmployeeId = 1, FirstName = "Nancy" };
        var shipper = new Shipper { ShipperId = 3, CompanyName = "Federal Shipping" };

        var order = new Order
        {
            Customer = customer,
            Employee = employee,
            Shipper = shipper
        };

        order.Customer.Should().NotBeNull();
        order.Employee.Should().NotBeNull();
        order.Shipper.Should().NotBeNull();
        order.Customer!.CompanyName.Should().Be("Alfreds");
    }
}