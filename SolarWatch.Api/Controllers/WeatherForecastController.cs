using Microsoft.AspNetCore.Mvc;

namespace SolarWatch.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet("Get")]
    public IEnumerable<WeatherForecast> Get(DateOnly date)
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = date.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            })
            .ToArray();
    }
}
