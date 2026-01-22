using Microsoft.EntityFrameworkCore;
using SolarWatch.Api.Data;
using SolarWatch.Api.Models;

namespace SolarWatch.Api.Repositories;

public class CityRepository(SolarWatchDbContext context) : ICityRepository
{
    private readonly SolarWatchDbContext _context = context;

    public async Task<IEnumerable<City>> GetAll()
    {
        return await _context.Cities.Include(c => c.SolarData).ToListAsync();
    }

    public async Task<City> Create(City item)
    {
        _context.Cities.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<City?> Read(int id)
    {
        return await _context.Cities.Include(c => c.SolarData).FirstAsync(c => c.Id == id);
    }

    public async Task<bool> Update(int id, City item)
    {
        var exists = await _context.Cities.FindAsync(id);
        if (exists == null)
            return false;
        exists.Name = item.Name;
        exists.Latitude = item.Latitude;
        exists.Longitude = item.Longitude;
        
        await  _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city == null)
            return false;
        await _context.SaveChangesAsync();
        return true;
    }
}