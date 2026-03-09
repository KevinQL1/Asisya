using Asisya.Services.DTOs;
using Asisya.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asisya.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
    {
        var productId = await _productService.CreateProductAsync(dto);
        return Ok(new { message = $"Producto '{dto.ProductName}' creado con éxito.", id = productId });
    }

    [Authorize]
    [HttpPost("bulk")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BulkInsertProducts([FromQuery] int count = 100000)
    {
        await _productService.GenerateMassiveProductsAsync(count);
        return Ok(new { message = $"Se han insertado {count} productos exitosamente." });
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProductResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, [FromQuery] int? categoryId = null)
    {
        if (page < 1 || pageSize < 1) 
            return BadRequest(new ErrorResponseDto { StatusCode = 400, Message = "Paginación inválida. La página y el tamaño deben ser mayores a 0." });
        
        var result = await _productService.GetProductsAsync(page, pageSize, search, categoryId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) 
            return NotFound(new ErrorResponseDto { StatusCode = 404, Message = $"Producto con ID {id} no encontrado." });
        
        return Ok(product);
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
    {
        var updatedProduct = await _productService.UpdateProductAsync(id, dto);

        if (updatedProduct == null) 
            return NotFound(new ErrorResponseDto { StatusCode = 404, Message = $"Producto con ID {id} no encontrado." });

        return Ok(new 
        { 
            message = "Producto Actualizado Correctamente", 
            product = updatedProduct 
        });
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var success = await _productService.DeleteProductAsync(id);

        if (!success) 
        {
            return NotFound(new ErrorResponseDto 
            { 
                StatusCode = 404, 
                Message = $"Producto con ID {id} no encontrado." 
            });
        }

        return Ok(new 
        { 
            message = "Producto Borrado Correctamente", 
            idDeleted = id 
        });
    }
}