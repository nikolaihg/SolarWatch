using System.Text.Json;
using SolarWatch.Api.Data;
using SolarWatch.Api.DTOs;
using SolarWatch.Api.Repositories;

namespace SolarWatch.Api.Services;

public class SunriseSunsetService
    : ISunriseSunsetService
{
    private readonly ILogger<SunriseSunsetService> _logger;
    private readonly HttpClient _httpClient;
    private readonly ISolarDataRepository _solarDataRepository;
    private readonly ICityRepository _cityRepository;
    private readonly string _sunrisesunsetUrl;


    public SunriseSunsetService(IConfiguration config, ILogger<SunriseSunsetService> logger, HttpClient client, ISolarDataRepository solarDataRepository, ICityRepository cityRepository)
    {
        _logger = logger;
        _httpClient = client;
        _solarDataRepository = solarDataRepository;
        _cityRepository = cityRepository;
        _sunrisesunsetUrl = config["SUNRISESUNSET_URL"] ??
                            throw new ArgumentNullException("SUNRISESUNSET_URL not configured.");
    }

    public async Task<SunriseSunsetResult> GetSunriseSunset(double latitude, double longitude)
    {
        var url = BuildUrl(latitude, longitude);
        return await FetchSunriseSunset(url);
    }
    
    public async Task<SunriseSunsetResult> GetSunriseSunset(double latitude, double longitude, DateOnly date)
    {
        // Get city
        var city = await _cityRepository.ReadByCoordinates(latitude, longitude);
        if (city == null)
        {
            throw new InvalidOperationException($"City with coordinates {latitude} and coordinates {longitude} not found.");
        }
        // Try DB first
        var exists = await _solarDataRepository.ReadByCityAndDate(city.Id, date);
        if (exists != null)
        {
            _logger.LogInformation("Solar data found in DB for city: {city}, date: {date}", city, date);
            return new SunriseSunsetResult(Sunrise: exists.Sunrise, Sunset: exists.Sunset);
        }
        // Fallback to Sunrise-Sunset API
        var url = BuildUrl(latitude, longitude, date);
        var result = await FetchSunriseSunset(url);
        
        // Save to DB
        var solarEntity = new Solar
        {
            CityId = city.Id,
            Date = date,
            Sunrise = result.Sunrise,
            Sunset = result.Sunset
        };
        await _solarDataRepository.Create(solarEntity);
        _logger.LogInformation("Solar data saved to DB for city: {city}, date: {date}", city, date);
        return result;
    }

    private async Task<SunriseSunsetResult> FetchSunriseSunset(string url)
    {
        _logger.LogInformation("Calling sunrise-sunrise API with url: {url}", url);

        var response = await _httpClient.GetAsync(url);
        _logger.LogInformation("Sunrise API returned status code {StatusCode}", response.StatusCode);

        using var doc = JsonDocument.Parse(
            await response.Content.ReadAsStringAsync());

        var results = doc.RootElement.GetProperty("results");
        var sunrise = results.GetProperty("sunrise").GetString();
        var sunset = results.GetProperty("sunset").GetString();

        return new SunriseSunsetResult(
            Sunrise: sunrise ?? string.Empty,
            Sunset: sunset ?? string.Empty);
    }
    
    private string BuildUrl(double latitude, double longitude, DateOnly? date = null)
    {
        var baseUrl = $"{_sunrisesunsetUrl}?lat={latitude}&lng={longitude}";
        return date.HasValue
            ? $"{baseUrl}&date={date:yyyy-MM-dd}"
            : baseUrl;
    }
}