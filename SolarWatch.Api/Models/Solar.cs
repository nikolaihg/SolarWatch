using SolarWatch.Api.Models;

namespace SolarWatch.Api.Data;

public class Solar
{
    public int Id { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public DateOnly Date { get; set; }
    
    public int CityId { get; set; }
    public City City { get; set; }

    public Solar()
    {
        
    }
}