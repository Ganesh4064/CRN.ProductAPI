using AutoMapper;
using CRN.ProductAPI.Application.DTOs;
using CRN.ProductAPI.Application.Interfaces;
using CRN.ProductAPI.Domain.Entities;

namespace CRN.ProductAPI.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ItemService(
        IItemRepository itemRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _itemRepository = itemRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ItemDto>> GetAllAsync(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var items = await _itemRepository.GetAllAsync(search);

        var totalRecords = items.Count();

        var pagedItems = items
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var totalPages = (int)Math.Ceiling(
            totalRecords / (double)pageSize);

        return new PagedResult<ItemDto>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            Data = _mapper.Map<IEnumerable<ItemDto>>(pagedItems)
        };
    }

    public async Task<ItemDto?> GetByIdAsync(int id)
    {
        var item = await _itemRepository.GetByIdAsync(id);

        if (item == null)
            return null;

        return _mapper.Map<ItemDto>(item);
    }

    public async Task<ItemDto?> CreateAsync(CreateItemDto dto)
    {
        var product = await _productRepository.GetByIdAsync(dto.ProductId);

        if (product == null)
            return null;

        var item = _mapper.Map<Item>(dto);

        var createdItem = await _itemRepository.CreateAsync(item);

        return _mapper.Map<ItemDto>(createdItem);
    }

    public async Task<ItemDto?> UpdateAsync(
        int id,
        UpdateItemDto dto)
    {
        var existingItem = await _itemRepository.GetByIdAsync(id);

        if (existingItem == null)
            return null;

        existingItem.Quantity = dto.Quantity;

        var updatedItem =
            await _itemRepository.UpdateAsync(existingItem);

        return _mapper.Map<ItemDto>(updatedItem);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _itemRepository.DeleteAsync(id);
    }
}