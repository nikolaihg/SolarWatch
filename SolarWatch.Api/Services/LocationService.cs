using System.Text.Json;
using SolarWatch.Api.DTOs;

namespace SolarWatch.Api.Services;

public class LocationService(IConfiguration config, ILogger<LocationService> logger, HttpClient client)
    : ILocationService
{
    private readonly string _openWeatherUrl = config["OPENWEATHER_URL"] ??
                                              throw new InvalidOperationException("OpenWeather URL not configured.");
    private readonly string _openWeatherApiKey = config["OPENWEATHER_API_KEY"] ??
                                                 throw new InvalidOperationException("OpenWeather API URL not configured.");

    public async Task<LatitudeLongitudeResult> GetCordinates(string city)
    {
        logger.LogInformation("Calling OpenWeather API with params: {city}", city);
        var resp = await client.GetAsync($"{_openWeatherUrl}?q={city}&appid={_openWeatherApiKey}");

        logger.LogInformation("OpenWeather API returned status code {StatusCode}", resp.StatusCode);

        using var jsondoc = JsonDocument.Parse(
            await resp.Content.ReadAsStringAsync());

        var root = jsondoc.RootElement;
        
        if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
        {
            logger.LogWarning("OpenWeather returned no results for city: {city}", city);
            throw new KeyNotFoundException($"City '{city}' not found.");
        }

        var firstResult = root[0];

        var latitude = firstResult.GetProperty("lat").GetDouble();
        var longitude = firstResult.GetProperty("lon").GetDouble();

        logger.LogInformation(
            "OpenWeather API returned latitude: {latitude}, longitude: {longitude}",
            latitude, longitude);
        
        return new LatitudeLongitudeResult(latitude, longitude);
    }
}