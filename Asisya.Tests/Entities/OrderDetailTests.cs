using Asisya.Entity;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Entities;

public class OrderDetailTests
{
    [Fact]
    public void OrderDetail_Should_Set_Properties_Correctly()
    {
        var orderDetail = new OrderDetail
        {
            OrderId = 10248,
            ProductId = 11,
            UnitPrice = 14.0m,
            Quantity = 12,
            Discount = 0.15f
        };

        orderDetail.OrderId.Should().Be(10248);
        orderDetail.ProductId.Should().Be(11);
        orderDetail.UnitPrice.Should().Be(14.0m);
        orderDetail.Quantity.Should().Be(12);
        orderDetail.Discount.Should().Be(0.15f);
    }

    [Fact]
    public void OrderDetail_Should_Link_To_Order_And_Product()
    {
        var order = new Order { OrderId = 10248 };
        var product = new Product { ProductId = 11, ProductName = "Queso Cabrales" };

        var orderDetail = new OrderDetail
        {
            OrderId = 10248,
            ProductId = 11,
            Order = order,
            Product = product
        };

        orderDetail.Order.Should().NotBeNull();
        orderDetail.Product.Should().NotBeNull();
        orderDetail.Order!.OrderId.Should().Be(10248);
        orderDetail.Product!.ProductName.Should().Be("Queso Cabrales");
    }
}