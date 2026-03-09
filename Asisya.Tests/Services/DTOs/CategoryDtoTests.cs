using Asisya.Services.DTOs;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Services.DTOs;

public class CategoryDtoTests
{
    [Fact]
    public void CategoryDto_Should_Initialize_With_Default_Values()
    {
        var dto = new CategoryDto();

        dto.CategoryName.Should().Be(string.Empty);
        dto.Description.Should().BeNull();
        dto.Picture.Should().BeNull();
    }

    [Fact]
    public void CategoryDto_Should_Set_Properties_Correctly()
    {
        var dto = new CategoryDto
        {
            CategoryName = "SERVIDORES",
            Description = "Infraestructura física para procesamiento",
            Picture = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgA..."
        };

        dto.CategoryName.Should().Be("SERVIDORES");
        dto.Description.Should().Be("Infraestructura física para procesamiento");
        dto.Picture.Should().NotBeNullOrWhiteSpace();
    }
}