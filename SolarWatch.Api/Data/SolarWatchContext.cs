using Microsoft.EntityFrameworkCore;
using SolarWatch.Api.Models;

namespace SolarWatch.Api.Data;

public class SolarWatchDbContext(DbContextOptions<SolarWatchDbContext> options) : DbContext(options)
{
    public DbSet<City> Cities { get; set; }
    public DbSet<Solar> Solars { get; set; }

    /*
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unique solar data entry per city and date
        modelBuilder.Entity<Solar>()
            .HasIndex(s => new { s.CityId, s.Date })
            .IsUnique();

        modelBuilder.Entity<Solar>()
            .HasOne(s => s.City)
            .WithMany(c => c.SolarData)
            .HasForeignKey(s => s.CityId)
            .IsRequired();

        // City names are unique
        modelBuilder.Entity<City>()
            .HasIndex(c => c.Name)
            .IsUnique();
        
        // max length
        modelBuilder.Entity<City>()
            .Property(c => c.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
    */
}