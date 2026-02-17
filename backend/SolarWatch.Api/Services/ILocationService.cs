using SolarWatch.Api.DTOs;

namespace SolarWatch.Api.Services;

public interface ILocationService
{
    Task<LatitudeLongitudeResult> GetCordinates(string city);
}