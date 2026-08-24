using CRN.ProductAPI.Application.DTOs;
using CRN.ProductAPI.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRN.ProductAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IValidator<CreateProductDto> _createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator;

    public ProductsController(
        IProductService productService,
        IValidator<CreateProductDto> createValidator,
        IValidator<UpdateProductDto> updateValidator)
    {
        _productService = productService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetAll(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            return BadRequest("Page number must be greater than 0.");
        }

        if (pageSize < 1)
        {
            return BadRequest("Page size must be greater than 0.");
        }

        if (pageSize > 100)
        {
            return BadRequest("Page size cannot be greater than 100.");
        }

        var products = await _productService.GetAllAsync(
            search,
            pageNumber,
            pageSize);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(
        CreateProductDto dto)
    {
        var validationResult =
            await _createValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var product = await _productService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDto>> Update(
        int id,
        UpdateProductDto dto)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var product =
            await _productService.UpdateAsync(id, dto);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _productService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}