using System.Text.Json;
using SolarWatch.Api.DTOs;
using SolarWatch.Api.Models;
using SolarWatch.Api.Repositories;

namespace SolarWatch.Api.Services;

public class LocationService : ILocationService
{
    private readonly ILogger<LocationService> _logger;
    private readonly HttpClient _client;
    private readonly ICityRepository _cityRepository;
    private readonly string _openWeatherUrl;
    private readonly string _openWeatherApiKey;

    public LocationService(IConfiguration config, ILogger<LocationService> logger, HttpClient client,
        ICityRepository cityRepository)
    {
        _logger = logger;
        _client = client;
        _cityRepository = cityRepository;
        _openWeatherUrl = config["OpenWeather:BaseUrl"] ??
                          throw new ArgumentNullException("OpenWeather URL not configured.");
        _openWeatherApiKey = config["OpenWeather:ApiKey"] ??
                             throw new ArgumentNullException("OpenWeather API Key not configured.");
    }

    public async Task<LatitudeLongitudeResult> GetCordinates(string city)
    {
        // Try DB first
        var exists = await _cityRepository.ReadByName(city);
        if (exists != null)
        {
            _logger.LogInformation("City found in database: {city}", city);
            return new LatitudeLongitudeResult(exists.Latitude, exists.Longitude);
        }

        // Fallback to OpenWeather API
        _logger.LogInformation("Calling OpenWeather API with params: {city}", city);
        var resp = await _client.GetAsync($"{_openWeatherUrl}?q={city}&appid={_openWeatherApiKey}");
        if (!resp.IsSuccessStatusCode)
        {
            _logger.LogError("OpenWeather API failed: {StatusCode}", resp.StatusCode);
            throw new Exception("Failed to fetch location data.");
        }

        _logger.LogInformation("OpenWeather API returned status code {StatusCode}", resp.StatusCode);

        using var jsondoc = JsonDocument.Parse(
            await resp.Content.ReadAsStringAsync());

        var root = jsondoc.RootElement;

        if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
        {
            _logger.LogWarning("OpenWeather returned no results for city: {city}", city);
            throw new KeyNotFoundException($"City '{city}' not found.");
        }

        var firstResult = root[0];
        var latitude = firstResult.GetProperty("lat").GetDouble();
        var longitude = firstResult.GetProperty("lon").GetDouble();
        string? country = firstResult.TryGetProperty("country", out var countryProp)
            ? countryProp.GetString()
            : string.Empty;

        string? state = firstResult.TryGetProperty("state", out var stateProp)
            ? stateProp.GetString()
            : string.Empty;

        _logger.LogInformation(
            "OpenWeather API returned latitude: {latitude}, longitude: {longitude}",
            latitude, longitude);
        
        // Save to DB
        var newCity = new City
        {
            Name = city,
            Latitude = latitude,
            Longitude = longitude,
            Country = country,
            State = state
        };
        await _cityRepository.Create(newCity);
        _logger.LogInformation("City saved to database: {city}", newCity.Name);

        return new LatitudeLongitudeResult(latitude, longitude);
    }
}