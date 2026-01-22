using SolarWatch.Api.Data;

namespace SolarWatch.Api.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public List<Solar> SolarData { get; set; } = new();
    
    public City()
    {
    }
}