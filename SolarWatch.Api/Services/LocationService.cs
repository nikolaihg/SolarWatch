using System.Text.Json;
using SolarWatch.Api.DTOs;

namespace SolarWatch.Api.Services;

public class LocationService : ILocationService
{
    private readonly ILogger<LocationService> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _openWeatherUrl;
    private readonly string _openWeatherApiKey;

    public LocationService(IConfiguration config, ILogger<LocationService> logger, HttpClient client)
    {
        _openWeatherApiKey = config["OPENWEATHER_API_KEY"] ??
                             throw new InvalidOperationException("OpenWeather API URL not configured.");
        _openWeatherUrl = config["OPENWEATHER_URL"] ??
                          throw new InvalidOperationException("OpenWeather URL not configured.");
        _httpClient = client;
        _logger = logger;
    }

    public async Task<LatitudeLongitudeResult> GetCordinates(string city)
    {
        _logger.LogInformation("Calling OpenWeather API with params: {city}", city);
        var resp = await _httpClient.GetAsync($"{_openWeatherUrl}/direct?q={city}&appid={_openWeatherApiKey}");

        _logger.LogInformation("OpenWeather API returned status code {StatusCode}", resp.StatusCode);

        using var jsondoc = JsonDocument.Parse(
            await resp.Content.ReadAsStringAsync());

        var root = jsondoc.RootElement;

        Console.WriteLine(root);

        if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
        {
            _logger.LogWarning("OpenWeather returned no results for city: {city}", city);
            throw new KeyNotFoundException($"City '{city}' not found.");
        }

        var firstResult = root[0];

        var latitude = firstResult.GetProperty("lat").GetDouble();
        var longitude = firstResult.GetProperty("lon").GetDouble();

        _logger.LogInformation(
            "OpenWeather API returned latitude: {latitude}, longitude: {longitude}",
            latitude, longitude);
        
        return new LatitudeLongitudeResult(latitude, longitude);
    }
}