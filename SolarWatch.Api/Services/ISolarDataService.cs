using SolarWatch.Api.DTOs;

namespace SolarWatch.Api.Services;

public interface ISolarDataService
{
    Task<IEnumerable<SolarDto>> GetAllSolarData();
    Task<SolarDto?> GetSolarDataById(int id);
    Task<SolarDto> CreateSolarData(SolarDto solarDto);
    Task<bool> UpdateSolarData(int id, SolarDto solarDto);
    Task<bool> DeleteSolarData(int id);
}
