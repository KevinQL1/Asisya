using System.Net;
using System.Net.Http.Json;
using Asisya.Services.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using DotNetEnv;

namespace Asisya.Tests.Api.Controllers;

public class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(CustomWebApplicationFactory factory)
    {
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", ".env");
        if (File.Exists(envPath)) Env.Load(envPath);

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Auth_Flow_Should_Register_And_Login_Successfully()
    {
        var registerDto = new RegisterRequestDto
        {
            Username = $"tester_{Guid.NewGuid().ToString()[..4]}",
            Password = "SecurePassword123!",
            FirstName = "Test",
            LastName = "User",
            City = "Envigado",
            Country = "Colombia"
        };

        var regResponse = await _client.PostAsJsonAsync("/api/Auth/register", registerDto);

        regResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginDto = new LoginRequestDto 
        { 
            Username = registerDto.Username, 
            Password = registerDto.Password 
        };
        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginDto);

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var authData = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
        
        authData.Should().NotBeNull();
        authData!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_Should_Return_401_With_Invalid_Credentials()
    {
        var invalidLogin = new LoginRequestDto { Username = "ghost", Password = "wrongpassword" };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", invalidLogin);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        error!.Message.Should().Be("Credenciales inválidas.");
    }
}