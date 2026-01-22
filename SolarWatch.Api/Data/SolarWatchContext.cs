using Microsoft.EntityFrameworkCore;
using SolarWatch.Api.Models;

namespace SolarWatch.Api.Data;

public class SolarWatchDbContext(DbContextOptions<SolarWatchDbContext> options) : DbContext(options)
{
    public DbSet<City> Cities { get; set; }
    public DbSet<Solar> Solars { get; set; }
}