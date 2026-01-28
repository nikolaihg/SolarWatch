namespace SolarWatch.Api.DTOs;

public class SolarDto
{
    public int Id { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public DateOnly Date { get; set; }
    public int CityId { get; set; }
}
