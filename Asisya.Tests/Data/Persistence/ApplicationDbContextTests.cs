using Asisya.Data.Persistence;
using Asisya.Entity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Asisya.Tests.Data.Persistence;

public class ApplicationDbContextTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task DbContext_ShouldSaveAndRetrieveProductWithCategory()
    {
        using var context = GetDbContext();
        var category = new Category { CategoryName = "Hardware" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var product = new Product 
        { 
            ProductName = "Mouse", 
            CategoryId = category.CategoryId,
            UnitPrice = 25.50m 
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var retrievedProduct = await context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductName == "Mouse");

        retrievedProduct.Should().NotBeNull();
        retrievedProduct!.Category.Should().NotBeNull();
        retrievedProduct.Category!.CategoryName.Should().Be("Hardware");
    }

    [Fact]
    public async Task Employee_RecursiveRelationship_ShouldWork()
    {
        using var context = GetDbContext();
        var manager = new Employee { FirstName = "Boss", LastName = "Main", Username = "boss" };
        context.Employees.Add(manager);
        await context.SaveChangesAsync();

        var subordinate = new Employee 
        { 
            FirstName = "Staff", 
            LastName = "Junior", 
            Username = "staff",
            ReportsTo = manager.EmployeeId 
        };
        context.Employees.Add(subordinate);
        await context.SaveChangesAsync();

        var retrievedManager = await context.Employees
            .Include(e => e.Subordinates)
            .FirstOrDefaultAsync(e => e.EmployeeId == manager.EmployeeId);

        retrievedManager!.Subordinates.Should().ContainSingle();
        retrievedManager.Subordinates.First().FirstName.Should().Be("Staff");
    }

    [Fact]
    public async Task OrderDetail_CompositeKey_ShouldBeConfigured()
    {
        using var context = GetDbContext();
        var orderDetail = new OrderDetail 
        { 
            OrderId = 10, 
            ProductId = 20, 
            UnitPrice = 10.0m, 
            Quantity = 1 
        };
        context.OrderDetails.Add(orderDetail);
        await context.SaveChangesAsync();

        var retrieved = await context.OrderDetails.FindAsync(10, 20);

        retrieved.Should().NotBeNull();
        retrieved!.OrderId.Should().Be(10);
        retrieved.ProductId.Should().Be(20);
    }
}