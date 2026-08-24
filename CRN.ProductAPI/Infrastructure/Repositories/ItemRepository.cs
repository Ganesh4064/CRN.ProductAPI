using CRN.ProductAPI.Application.Interfaces;
using CRN.ProductAPI.Domain.Entities;
using CRN.ProductAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRN.ProductAPI.Infrastructure.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly ApplicationDbContext _context;

    public ItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Item>> GetAllAsync(
        string? search = null)
    {
        var query = _context.Items
            .AsNoTracking()
            .Include(i => i.Product)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(i =>
                i.Product.ProductName.Contains(search));
        }

        return await query.ToListAsync();
    }

    public async Task<Item?> GetByIdAsync(int id)
    {
        return await _context.Items
            .AsNoTracking()
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Item> CreateAsync(Item item)
    {
        _context.Items.Add(item);

        await _context.SaveChangesAsync();

        return item;
    }

    public async Task<Item?> UpdateAsync(Item item)
    {
        var existingItem = await _context.Items
            .FirstOrDefaultAsync(i => i.Id == item.Id);

        if (existingItem == null)
            return null;

        existingItem.Quantity = item.Quantity;

        await _context.SaveChangesAsync();

        return existingItem;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _context.Items
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null)
            return false;

        _context.Items.Remove(item);

        await _context.SaveChangesAsync();

        return true;
    }
}