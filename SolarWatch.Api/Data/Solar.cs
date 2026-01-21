using SolarWatch.Api.Models;

namespace SolarWatch.Api.Data;

public class Solar
{
    public int Id { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public DateOnly date { get; set; }
    
    public int CityId { get; set; }
    public City city { get; set; }

    public Solar()
    {
        
    }
}