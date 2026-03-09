using Asisya.Services.DTOs;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Services.DTOs;

public class ErrorResponseDtoTests
{
    [Fact]
    public void ErrorResponseDto_Should_Initialize_With_Default_Values()
    {
        var dto = new ErrorResponseDto();

        dto.Message.Should().Be(string.Empty);
        dto.StatusCode.Should().Be(0);
        dto.Details.Should().BeNull();
    }

    [Fact]
    public void ErrorResponseDto_Should_Set_Properties_Correctly()
    {
        var dto = new ErrorResponseDto
        {
            StatusCode = 404,
            Message = "Producto no encontrado",
            Details = "El ID proporcionado no existe en la base de datos."
        };

        dto.StatusCode.Should().Be(404);
        dto.Message.Should().Be("Producto no encontrado");
        dto.Details.Should().Be("El ID proporcionado no existe en la base de datos.");
    }
}