using SolarWatch.Api.DTOs;

namespace SolarWatch.Api.Services;

public interface ICityService
{
    Task<IEnumerable<CityDto>> GetAllCities();
    Task<IEnumerable<CityNameDto>> GetAllCityNames();
    Task<CityDto?> GetCityById(int id);
    Task<CityDto> CreateCity(CityDto cityDto);
    Task<bool> UpdateCity(int id, CityDto cityDto);
    Task<bool> DeleteCity(int id);
}
