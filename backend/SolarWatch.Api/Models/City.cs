using SolarWatch.Api.Data;

namespace SolarWatch.Api.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Country { get; set; }
    public string State { get; set; }

    public List<Solar> SolarData { get; set; } = new();
    
    public City()
    {
    }

    public City(string name, double latitude, double longitude, string country, string state)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Country = country;
        State = state;
    }
}