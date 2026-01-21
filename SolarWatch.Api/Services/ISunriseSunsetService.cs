using SolarWatch.Api.DTOs;

namespace SolarWatch.Api.Services;

public interface ISunriseSunsetService
{
    Task<SunriseSunsetResult> GetSunriseSunset(double latitude, double longitude);
}