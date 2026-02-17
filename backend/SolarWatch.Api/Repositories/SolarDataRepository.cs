using Microsoft.EntityFrameworkCore;
using SolarWatch.Api.Data;

namespace SolarWatch.Api.Repositories;

public class SolarDataRepository(AppDbContext context) : ISolarDataRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<IEnumerable<Solar>> GetAll()
    {
        return await _context.Solars.Include(s => s.City).ToListAsync();
    }

    public async Task<Solar> Create(Solar item)
    {
        _context.Solars.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<Solar?> Read(int id)
    {
        return await _context.Solars.Include(s => s.City).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Solar?> ReadByCityAndDate(int cityId, DateOnly date)
    {
        return await _context.Solars
            .Include(s => s.City)
            .FirstOrDefaultAsync(s =>
                s.CityId == cityId &&
                s.Date == date);
    }

    public async Task<bool> Update(int id, Solar item)
    {
        var exists = await _context.Solars.FindAsync(id);
        if (exists == null)
            return false;
        exists.Sunrise = item.Sunrise;
        exists.Sunset = item.Sunset;
        exists.Date = item.Date;
        exists.CityId = item.CityId;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var solar = await _context.Solars.FindAsync(id);
        if (solar == null)
            return false;

        _context.Solars.Remove(solar);
        await _context.SaveChangesAsync();
        return true;
    }
}