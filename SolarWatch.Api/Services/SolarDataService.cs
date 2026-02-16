using SolarWatch.Api.Data;
using SolarWatch.Api.DTOs;
using SolarWatch.Api.Repositories;

namespace SolarWatch.Api.Services;

public class SolarDataService : ISolarDataService
{
    private readonly ISolarDataRepository _solarDataRepository;

    public SolarDataService(ISolarDataRepository solarDataRepository)
    {
        _solarDataRepository = solarDataRepository;
    }

    public async Task<IEnumerable<SolarDto>> GetAllSolarData()
    {
        var solarData = await _solarDataRepository.GetAll();
        return solarData.Select(MapToDto).ToList();
    }

    public async Task<SolarDto?> GetSolarDataById(int id)
    {
        var solar = await _solarDataRepository.Read(id);
        return solar == null ? null : MapToDto(solar);
    }

    public async Task<SolarDto> CreateSolarData(SolarDto solarDto)
    {
        var solar = MapToEntity(solarDto);
        var createdSolar = await _solarDataRepository.Create(solar);
        return MapToDto(createdSolar);
    }

    public async Task<bool> UpdateSolarData(int id, SolarDto solarDto)
    {
        var solar = MapToEntity(solarDto);
        return await _solarDataRepository.Update(id, solar);
    }

    public async Task<bool> DeleteSolarData(int id)
    {
        return await _solarDataRepository.Delete(id);
    }

    private static SolarDto MapToDto(Solar solar)
    {
        return new SolarDto
        {
            Id = solar.Id,
            Sunrise = solar.Sunrise,
            Sunset = solar.Sunset,
            Date = solar.Date,
            CityId = solar.CityId
        };
    }

    private static Solar MapToEntity(SolarDto solarDto)
    {
        return new Solar
        {
            Sunrise = solarDto.Sunrise,
            Sunset = solarDto.Sunset,
            Date = solarDto.Date,
            CityId = solarDto.CityId
        };
    }
}
