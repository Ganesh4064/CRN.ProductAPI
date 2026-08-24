using CRN.ProductAPI.Application.DTOs;

namespace CRN.ProductAPI.Application.Interfaces;

public interface IItemService
{
    Task<PagedResult<ItemDto>> GetAllAsync(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10);

    Task<ItemDto?> GetByIdAsync(int id);

    Task<ItemDto?> CreateAsync(CreateItemDto dto);

    Task<ItemDto?> UpdateAsync(
        int id,
        UpdateItemDto dto);

    Task<bool> DeleteAsync(int id);
}