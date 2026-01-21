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
        var url = BuildUrl(latitude, longitude);
        return await FetchSunriseSunset(url);
    }
    
    public async Task<SunriseSunsetResult> GetSunriseSunset(double latitude, double longitude, DateOnly date)
    {
        var url = BuildUrl(latitude, longitude, date);
        return await FetchSunriseSunset(url);

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