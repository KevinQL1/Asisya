using Asisya.Services.DTOs;

namespace Asisya.Services.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequestDto dto);
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
}