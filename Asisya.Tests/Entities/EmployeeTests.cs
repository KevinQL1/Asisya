using Asisya.Entity;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Entities;

public class EmployeeTests
{
    [Fact]
    public void Employee_Should_Initialize_With_Default_Values()
    {
        var employee = new Employee();

        employee.LastName.Should().Be(string.Empty);
        employee.FirstName.Should().Be(string.Empty);
        employee.Username.Should().Be(string.Empty);
        employee.PasswordHash.Should().Be(string.Empty);
        employee.Subordinates.Should().NotBeNull();
        employee.Orders.Should().NotBeNull();
    }

    [Fact]
    public void Employee_Should_Set_Properties_Correctly()
    {
        var birthDate = new DateTime(1990, 1, 1);
        var hireDate = DateTime.UtcNow;
        var photoData = new byte[] { 0x20, 0x20, 0x20 };

        var employee = new Employee
        {
            EmployeeId = 1,
            FirstName = "Kevin",
            LastName = "Dev",
            Username = "kevin.dev",
            PasswordHash = "hashed_pw",
            BirthDate = birthDate,
            HireDate = hireDate,
            City = "Envigado",
            Country = "Colombia",
            Photo = photoData
        };

        employee.EmployeeId.Should().Be(1);
        employee.FirstName.Should().Be("Kevin");
        employee.LastName.Should().Be("Dev");
        employee.Username.Should().Be("kevin.dev");
        employee.BirthDate.Should().Be(birthDate);
        employee.HireDate.Should().Be(hireDate);
        employee.City.Should().Be("Envigado");
        employee.Photo.Should().BeEquivalentTo(photoData);
    }

    [Fact]
    public void Employee_RecursiveRelationship_Should_Maintain_Hierarchy()
    {
        var manager = new Employee { EmployeeId = 1, FirstName = "Boss" };
        var subordinate = new Employee 
        { 
            EmployeeId = 2, 
            FirstName = "Worker", 
            ReportsTo = 1, 
            Manager = manager 
        };

        manager.Subordinates.Add(subordinate);

        manager.Subordinates.Should().HaveCount(1);
        subordinate.Manager.Should().NotBeNull();
        subordinate.Manager!.FirstName.Should().Be("Boss");
        subordinate.ReportsTo.Should().Be(1);
    }
}