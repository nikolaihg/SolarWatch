using System.Text.Json;
using SolarWatch.Api.DTOs;

namespace SolarWatch.Api.Services;

public class SunriseSunsetService
    : ISunriseSunsetService
{
    private readonly ILogger<SunriseSunsetService> _logger;
    private readonly HttpClient _httpClient;

    private readonly string _sunrisesunsetUrl;


    public SunriseSunsetService(IConfiguration config, ILogger<SunriseSunsetService> logger, HttpClient client)
    {
        _sunrisesunsetUrl = config["SUNRISESUNSET_URL"] ??
                            throw new ArgumentNullException("SUNRISESUNSET_URL not configured.");
        _logger = logger;
        _httpClient = client;
    }

    public async Task<SunriseSunsetResult> GetSunriseSunset(double latitude, double longitude)
    {
        var url = $"{_sunrisesunsetUrl}?lat={latitude}&lng={longitude}";

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
    
    public async Task<SunriseSunsetResult> GetSunriseSunset(double latitude, double longitude, DateOnly date)
    {
        var url = $"{_sunrisesunsetUrl}?lat={latitude}&lng={longitude}&date={date:yy-MM-dd}";

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
}