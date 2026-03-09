namespace Asisya.Services.DTOs;

// Este será el esquema para cualquier error
public class ErrorResponseDto
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
}