using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asisya.Data.Persistence;
using Asisya.Entity;
using Asisya.Services.DTOs;
using Asisya.Services.Interfaces;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Asisya.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto dto)
    {
        try 
        {
            if (await _context.Employees.AnyAsync(x => x.Username == dto.Username))
                return false;

            var employee = new Employee
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Title = dto.Title,
                TitleOfCourtesy = dto.TitleOfCourtesy,
                
                BirthDate = dto.BirthDate.HasValue 
                    ? DateTime.SpecifyKind(dto.BirthDate.Value, DateTimeKind.Utc) 
                    : null,
                
                HireDate = dto.HireDate.HasValue 
                    ? DateTime.SpecifyKind(dto.HireDate.Value, DateTimeKind.Utc) 
                    : DateTime.UtcNow,

                Address = dto.Address,
                City = dto.City,
                Region = dto.Region,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                HomePhone = dto.HomePhone,
                Extension = dto.Extension,
                Notes = dto.Notes,
                ReportsTo = dto.ReportsTo
            };

            if (!string.IsNullOrEmpty(dto.PhotoBase64))
            {
                try {
                    employee.Photo = Convert.FromBase64String(dto.PhotoBase64);
                } catch (FormatException) {
                    employee.Photo = null;
                }
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al registrar el empleado en la base de datos.", ex);
        }
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        try 
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (employee == null || !BCrypt.Net.BCrypt.Verify(dto.Password, employee.PasswordHash))
                return null;

            return new LoginResponseDto
            {
                Token = GenerateJwtToken(employee),
                Expiration = DateTime.UtcNow.AddHours(3).ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
        catch (Exception ex)
        {
            throw new Exception("Error durante el proceso de autenticación.", ex);
        }
    }

    private string GenerateJwtToken(Employee employee)
    {
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, employee.Username),
            new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
            new Claim(ClaimTypes.Name, $"{employee.FirstName} {employee.LastName}"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("JWT_KEY")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Env.GetString("JWT_ISSUER"),
            audience: Env.GetString("JWT_AUDIENCE"),
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}