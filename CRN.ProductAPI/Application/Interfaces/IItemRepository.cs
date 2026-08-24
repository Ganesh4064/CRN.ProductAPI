using CRN.ProductAPI.Domain.Entities;

namespace CRN.ProductAPI.Application.Interfaces;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetAllAsync(
        string? search = null);

    Task<Item?> GetByIdAsync(int id);

    Task<Item> CreateAsync(Item item);

    Task<Item?> UpdateAsync(Item item);

    Task<bool> DeleteAsync(int id);
}