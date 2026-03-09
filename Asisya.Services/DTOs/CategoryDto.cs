namespace Asisya.Services.DTOs;
using System.ComponentModel.DataAnnotations;

public class CategoryDto
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
    public string CategoryName { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public string? Picture { get; set; }
}