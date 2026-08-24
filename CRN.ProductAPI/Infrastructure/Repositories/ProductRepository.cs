using CRN.ProductAPI.Application.Interfaces;
using CRN.ProductAPI.Domain.Entities;
using CRN.ProductAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRN.ProductAPI.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(
        string? search = null)
    {
        var query = _context.Products
            .AsNoTracking()
            .Include(p => p.Items)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.ProductName.Contains(search));
        }

        return await query.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        var existingProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.ProductName = product.ProductName;
        existingProduct.ModifiedBy = product.ModifiedBy;
        existingProduct.ModifiedOn = product.ModifiedOn;

        await _context.SaveChangesAsync();

        return existingProduct;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return true;
    }
}