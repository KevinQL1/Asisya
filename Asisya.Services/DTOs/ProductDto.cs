namespace Asisya.Services.DTOs;

// DTO para guardar un solo producto
public class ProductCreateDto
{
    public string ProductName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
}

// DTO para la lista general
public class ProductResponseDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? CategoryPicture { get; set; }
}

// DTO para el endpoint de Detalle
public class ProductDetailDto : ProductResponseDto
{
    public string? QuantityPerUnit { get; set; }
    public bool Discontinued { get; set; }
}

// DTO para recibir los datos del UPDATE (PUT)
public class ProductUpdateDto
{
    public string ProductName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
}

// Clase genérica para estandarizar la paginación en toda la API
public class PagedResult<T>
{
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public List<T> Data { get; set; } = new List<T>();
}
