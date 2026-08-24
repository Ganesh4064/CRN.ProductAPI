using CRN.ProductAPI.Domain.Entities;

namespace CRN.ProductAPI.Application.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(
        string? search = null);

    Task<Product?> GetByIdAsync(int id);

    Task<Product> CreateAsync(Product product);

    Task<Product?> UpdateAsync(Product product);

    Task<bool> DeleteAsync(int id);
}