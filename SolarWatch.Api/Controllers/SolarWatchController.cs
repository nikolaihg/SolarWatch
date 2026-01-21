using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Api.DTOs;

namespace SolarWatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private HttpClient _client ;
    private readonly string _openWeatherApiKey;
    private readonly string _openWeatherUrl;
    private readonly string _sunrisesunsetUrl;

    public SolarWatchController(IConfiguration config, ILogger<SolarWatchController> logger, IHttpClientFactory http)
    {
        _openWeatherApiKey = config["OPENWEATHER_API_KEY"] ??
                             throw new InvalidOperationException("OPENWEATHER_API_KEY not configured.");
        _openWeatherUrl = config["OPENWEATHER_URL"] ??
                          throw new InvalidOperationException("OPENWEATHER_URL not configured.");
        _sunrisesunsetUrl = config["SUNRISESUNSET_URL"] ??
                            throw new InvalidOperationException("SUNRISESUNSET_URL not configured.");
        
        _logger = logger;
        _client = http.CreateClient();
    }

    [HttpGet]
    public async Task<ActionResult<SunriseSunsetResult>> Get(string city)
    {
        _logger.LogInformation("Calling OpenWeather API with params: {city}", city);
        var resp = await _client.GetAsync($"{_openWeatherUrl}/direct?q={city}&appid={_openWeatherApiKey}");

        _logger.LogInformation("OpenWeather API returned status code {StatusCode}", resp.StatusCode);

        using var jsondoc = JsonDocument.Parse(
            await resp.Content.ReadAsStringAsync());
        
        var root = jsondoc.RootElement;

        Console.WriteLine(root);

        if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
        {
            _logger.LogWarning("OpenWeather returned no results for city: {city}", city);
            return NotFound($"City '{city}' not found.");
        }
        
        var firstResult = root[0];

        var latitude = firstResult.GetProperty("lat").GetDouble();
        var longitude = firstResult.GetProperty("lon").GetDouble();
        
        _logger.LogInformation(
            "OpenWeather API returned latitude: {latitude}, longitude: {longitude}",
            latitude, longitude);
        
        var url = $"{_sunrisesunsetUrl}/json?lat=" + latitude + "&lng=" + longitude;

        _logger.LogInformation("Calling sunrise-sunrise API with url: {url}", url);

        var response = await _client.GetAsync(url);
        _logger.LogInformation("Sunrise API returned status code {StatusCode}", response.StatusCode);

        using var doc = JsonDocument.Parse(
            await response.Content.ReadAsStringAsync());

        var results = doc.RootElement.GetProperty("results");
        var sunrise = results.GetProperty("sunrise").GetString();
        var sunset = results.GetProperty("sunset").GetString();

        var dto = new ApiResponse<SunriseSunsetResult>(
            Results: new SunriseSunsetResult(
                Sunrise: sunrise ?? string.Empty,
                Sunset: sunset ?? string.Empty),
            Status: doc.RootElement.GetProperty("status").GetString() ?? "UNKNOWN");

        return Ok(dto);
    }
}