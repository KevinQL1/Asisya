using System.Text;
using System.Runtime.CompilerServices;
using Asisya.Api.Middlewares;
using Asisya.Data.Persistence;
using Asisya.Services.Interfaces;
using Asisya.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DotNetEnv;

[assembly: InternalsVisibleTo("Asisya.Tests")]

var builder = WebApplication.CreateBuilder(args);

// 1. buscar y cargar el archivo .env
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
Env.Load(envPath);

// configurar el puerto dinámicamente desde el .env
builder.WebHost.UseUrls($"http://*:{Env.GetString("PORT_API")}");

// 2. construir la cadena de conexión dinámicamente
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    // si se utiliza dotnet run coge .ENV
    connectionString = $"Host={Env.GetString("DB_HOST_LOCAL")};" +
                       $"Port={Env.GetString("PORT_DB")};" +
                       $"Database={Env.GetString("DB_NAME")};" +
                       $"Username={Env.GetString("USER")};" +
                       $"Password={Env.GetString("PASSWORD")}";
}

// 3. configurar la conexión a PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// 4. configurar de cors frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAsisyaApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 5. configurar la autenticación JWT
var jwtKey = Env.GetString("JWT_KEY");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Env.GetString("JWT_ISSUER"),
        ValidAudience = Env.GetString("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// 6. INYECCIÓN DE DEPENDENCIAS (arquitectura por capas)
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 7. configurar swagger para JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Asisya API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT. Ingrese 'Bearer' [espacio] y su token.\n\nEjemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    int retries = 10;
    while (retries > 0)
    {
        try
        {
            context.Database.Migrate();
            Console.WriteLine("base de datos migrada y tablas vacias creadas.");
            break; 
        }
        catch (Exception ex)
        {
            retries--;
            Console.WriteLine($"esperando db... reintentos: {retries}. Error: {ex.Message}");
            Thread.Sleep(3000); 
            if (retries == 0) throw; 
        }
    }
}

// 8. middleware de manejo de errores global
app.UseMiddleware<ExceptionMiddleware>();

// 9. activar CORS
app.UseCors("AllowAsisyaApp");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Asisya API v1");
    c.RoutePrefix = "swagger";
});

// 10. activar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

public partial class Program { }