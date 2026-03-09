using Asisya.Services.DTOs;
using FluentAssertions;
using Xunit;

namespace Asisya.Tests.Services.DTOs;

public class LoginDtoTests
{
    [Fact]
    public void LoginRequestDto_Should_Set_Properties_Correctly()
    {
        var dto = new LoginRequestDto
        {
            Username = "kevin.dev",
            Password = "SecurePassword123"
        };

        dto.Username.Should().Be("kevin.dev");
        dto.Password.Should().Be("SecurePassword123");
    }

    [Fact]
    public void LoginResponseDto_Should_Set_Properties_Correctly()
    {
        var expirationDate = DateTime.UtcNow.AddHours(1).ToString();
        var dto = new LoginResponseDto
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
            Expiration = expirationDate
        };

        dto.Token.Should().Be("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...");
        dto.Expiration.Should().Be(expirationDate);
    }
}