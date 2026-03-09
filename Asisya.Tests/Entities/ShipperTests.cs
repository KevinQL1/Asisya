using Asisya.Entity;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Entities;

public class ShipperTests
{
    [Fact]
    public void Shipper_Should_Initialize_With_Default_Values()
    {
        var shipper = new Shipper();

        shipper.CompanyName.Should().Be(string.Empty);
        shipper.Orders.Should().NotBeNull();
        shipper.Orders.Should().BeEmpty();
    }

    [Fact]
    public void Shipper_Should_Set_Properties_Correctly()
    {
        var shipper = new Shipper
        {
            ShipperId = 3,
            CompanyName = "Federal Shipping",
            Phone = "(503) 555-9931"
        };

        shipper.ShipperId.Should().Be(3);
        shipper.CompanyName.Should().Be("Federal Shipping");
        shipper.Phone.Should().Be("(503) 555-9931");
    }

    [Fact]
    public void Shipper_Should_Allow_Adding_Orders()
    {
        var shipper = new Shipper { ShipperId = 1, CompanyName = "Speedy Express" };
        var order = new Order { OrderId = 10248, Shipper = shipper };

        shipper.Orders.Add(order);

        shipper.Orders.Should().HaveCount(1);
        shipper.Orders.First().OrderId.Should().Be(10248);
    }
}