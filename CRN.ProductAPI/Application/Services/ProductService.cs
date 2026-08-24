using AutoMapper;
using CRN.ProductAPI.Application.DTOs;
using CRN.ProductAPI.Application.Interfaces;
using CRN.ProductAPI.Domain.Entities;

namespace CRN.ProductAPI.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductDto>> GetAllAsync(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var products = await _productRepository.GetAllAsync(search);

        var totalRecords = products.Count();

        var pagedProducts = products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var totalPages = (int)Math.Ceiling(
            totalRecords / (double)pageSize);

        return new PagedResult<ProductDto>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            Data = _mapper.Map<IEnumerable<ProductDto>>(pagedProducts)
        };
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return null;
        }

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);

        product.CreatedOn = DateTime.UtcNow;

        var createdProduct =
            await _productRepository.CreateAsync(product);

        return _mapper.Map<ProductDto>(createdProduct);
    }

    public async Task<ProductDto?> UpdateAsync(
        int id,
        UpdateProductDto dto)
    {
        var existingProduct =
            await _productRepository.GetByIdAsync(id);

        if (existingProduct == null)
        {
            return null;
        }

        _mapper.Map(dto, existingProduct);

        existingProduct.ModifiedOn = DateTime.UtcNow;

        var updatedProduct =
            await _productRepository.UpdateAsync(existingProduct);

        return updatedProduct == null
            ? null
            : _mapper.Map<ProductDto>(updatedProduct);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }
}