using CRN.ProductAPI.Application.DTOs;

namespace CRN.ProductAPI.Application.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetAllAsync(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10);

    Task<ProductDto?> GetByIdAsync(int id);

    Task<ProductDto> CreateAsync(CreateProductDto dto);

    Task<ProductDto?> UpdateAsync(
        int id,
        UpdateProductDto dto);

    Task<bool> DeleteAsync(int id);
}