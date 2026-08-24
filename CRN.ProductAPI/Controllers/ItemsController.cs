using CRN.ProductAPI.Application.DTOs;
using CRN.ProductAPI.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRN.ProductAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly IValidator<CreateItemDto> _createValidator;
    private readonly IValidator<UpdateItemDto> _updateValidator;

    public ItemsController(
        IItemService itemService,
        IValidator<CreateItemDto> createValidator,
        IValidator<UpdateItemDto> updateValidator)
    {
        _itemService = itemService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ItemDto>>> GetAll(
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

        var items = await _itemService.GetAllAsync(
            search,
            pageNumber,
            pageSize);

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemDto>> GetById(int id)
    {
        var item = await _itemService.GetByIdAsync(id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create(
        CreateItemDto dto)
    {
        var validationResult =
            await _createValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var item = await _itemService.CreateAsync(dto);

        if (item == null)
            return NotFound("Product not found.");

        return CreatedAtAction(
            nameof(GetById),
            new { id = item.Id },
            item);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ItemDto>> Update(
        int id,
        UpdateItemDto dto)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var item = await _itemService.UpdateAsync(id, dto);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _itemService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}