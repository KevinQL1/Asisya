using System.Net;
using System.Text.Json;
using Asisya.Api.Middlewares;
using Asisya.Services.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Asisya.Tests.Api.Middlewares;

public class ExceptionMiddlewareTests
{
    private readonly Mock<ILogger<ExceptionMiddleware>> _loggerMock;
    private readonly Mock<IHostEnvironment> _envMock;

    public ExceptionMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
        _envMock = new Mock<IHostEnvironment>();
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturnNotFound_WhenKeyNotFoundExceptionIsThrown()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        
        RequestDelegate next = (innerContext) => throw new KeyNotFoundException("Recurso no encontrado");
        var middleware = new ExceptionMiddleware(next, _loggerMock.Object, _envMock.Object);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        context.Response.ContentType.Should().Be("application/json");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        var result = JsonSerializer.Deserialize<ErrorResponseDto>(responseBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        result!.StatusCode.Should().Be(404);
        result.Message.Should().Be("Recurso no encontrado");
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        
        RequestDelegate next = (innerContext) => throw new ArgumentException("Datos inválidos");
        var middleware = new ExceptionMiddleware(next, _loggerMock.Object, _envMock.Object);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        var result = JsonSerializer.Deserialize<ErrorResponseDto>(responseBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        result!.StatusCode.Should().Be(400);
        result.Message.Should().Be("Datos inválidos");
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturnInternalServerError_WhenGenericExceptionIsThrown()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        _envMock.Setup(m => m.EnvironmentName).Returns("Production");

        RequestDelegate next = (innerContext) => throw new Exception("Error fatal");
        var middleware = new ExceptionMiddleware(next, _loggerMock.Object, _envMock.Object);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        var result = JsonSerializer.Deserialize<ErrorResponseDto>(responseBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        result!.Message.Should().Be("Ocurrió un error interno en el servidor.");
    }
}