using Asisya.Services.DTOs;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Services.DTOs;

public class RegisterRequestDtoTests
{
    [Fact]
    public void RegisterRequestDto_Should_Initialize_With_Default_Values()
    {
        var dto = new RegisterRequestDto();

        dto.Username.Should().Be(string.Empty);
        dto.Password.Should().Be(string.Empty);
        dto.FirstName.Should().Be(string.Empty);
        dto.LastName.Should().Be(string.Empty);
        dto.PhotoBase64.Should().BeNull();
    }

    [Fact]
    public void RegisterRequestDto_Should_Set_Properties_Correctly()
    {
        var birthDate = new DateTime(1985, 5, 20);
        var hireDate = DateTime.UtcNow;

        var dto = new RegisterRequestDto
        {
            Username = "kevin.dev",
            Password = "SecurePassword123",
            FirstName = "Kevin",
            LastName = "Developer",
            Title = "Senior Software Engineer",
            BirthDate = birthDate,
            HireDate = hireDate,
            City = "Envigado",
            Country = "Colombia",
            PhotoBase64 = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQ..."
        };

        dto.Username.Should().Be("kevin.dev");
        dto.FirstName.Should().Be("Kevin");
        dto.LastName.Should().Be("Developer");
        dto.BirthDate.Should().Be(birthDate);
        dto.HireDate.Should().Be(hireDate);
        dto.City.Should().Be("Envigado");
        dto.Country.Should().Be("Colombia");
        dto.PhotoBase64.Should().StartWith("data:image");
    }

    [Fact]
    public void RegisterRequestDto_Should_Handle_Optional_Hierarchy_Data()
    {
        var dto = new RegisterRequestDto
        {
            ReportsTo = 5,
            Notes = "Candidato seleccionado para el proyecto Finanzauto"
        };

        dto.ReportsTo.Should().Be(5);
        dto.Notes.Should().Be("Candidato seleccionado para el proyecto Finanzauto");
    }
}