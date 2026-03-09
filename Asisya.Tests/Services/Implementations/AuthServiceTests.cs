using Asisya.Data.Persistence;
using Asisya.Entity;
using Asisya.Services.DTOs;
using Asisya.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Asisya.Tests.Services.Implementations;

public class AuthServiceTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnTrue_WhenUserIsNew()
    {
        using var context = GetDbContext();
        var service = new AuthService(context);
        var dto = new RegisterRequestDto
        {
            Username = "newuser",
            Password = "Password123",
            FirstName = "Test",
            LastName = "User"
        };

        var result = await service.RegisterAsync(dto);

        result.Should().BeTrue();
        var userInDb = await context.Employees.FirstOrDefaultAsync(u => u.Username == "newuser");
        userInDb.Should().NotBeNull();
        BCrypt.Net.BCrypt.Verify("Password123", userInDb!.PasswordHash).Should().BeTrue();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnFalse_WhenUsernameExists()
    {
        using var context = GetDbContext();
        context.Employees.Add(new Employee { Username = "existing", PasswordHash = "any" });
        await context.SaveChangesAsync();

        var service = new AuthService(context);
        var dto = new RegisterRequestDto { Username = "existing", Password = "123" };

        var result = await service.RegisterAsync(dto);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterAsync_ShouldHandleBase64PhotoCorrectly()
    {
        using var context = GetDbContext();
        var service = new AuthService(context);
        // "R0lG" es el inicio de un base64 válido para un GIF
        var dto = new RegisterRequestDto 
        { 
            Username = "photouser", 
            Password = "123", 
            PhotoBase64 = "R0lGODlhAQABAIAAAAUEBAAAACwAAAAAAQABAAACAkQBADs=" 
        };

        await service.RegisterAsync(dto);

        var userInDb = await context.Employees.FirstAsync(u => u.Username == "photouser");
        userInDb.Photo.Should().NotBeNull();
        userInDb.Photo.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Nota: Asegúrate de tener un .env de prueba o configurar las variables de entorno para JWT
        Environment.SetEnvironmentVariable("JWT_KEY", "super_secret_key_of_at_least_32_characters_long");
        Environment.SetEnvironmentVariable("JWT_ISSUER", "TestIssuer");
        Environment.SetEnvironmentVariable("JWT_AUDIENCE", "TestAudience");

        using var context = GetDbContext();
        var password = "RealPassword123";
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        context.Employees.Add(new Employee { Username = "loginuser", PasswordHash = hash, FirstName = "A", LastName = "B" });
        await context.SaveChangesAsync();

        var service = new AuthService(context);
        var dto = new LoginRequestDto { Username = "loginuser", Password = password };

        var result = await service.LoginAsync(dto);

        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
    {
        using var context = GetDbContext();
        var hash = BCrypt.Net.BCrypt.HashPassword("CorrectOne");
        context.Employees.Add(new Employee { Username = "user", PasswordHash = hash });
        await context.SaveChangesAsync();

        var service = new AuthService(context);
        var dto = new LoginRequestDto { Username = "user", Password = "WrongPassword" };

        var result = await service.LoginAsync(dto);

        result.Should().BeNull();
    }
}