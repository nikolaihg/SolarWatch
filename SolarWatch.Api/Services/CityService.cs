using SolarWatch.Api.DTOs;
using SolarWatch.Api.Models;
using SolarWatch.Api.Repositories;

namespace SolarWatch.Api.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;

    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<IEnumerable<CityDto>> GetAllCities()
    {
        var cities = await _cityRepository.GetAll();
        return cities.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<CityNameDto>> GetAllCityNames()
    {
        var cities = await _cityRepository.GetAll();
        return cities.Select(c => new CityNameDto { Name = c.Name.ToLower() }).ToList();
    }

    public async Task<CityDto?> GetCityById(int id)
    {
        var city = await _cityRepository.Read(id);
        return city == null ? null : MapToDto(city);
    }

    public async Task<CityDto> CreateCity(CityDto cityDto)
    {
        var city = MapToEntity(cityDto);
        var createdCity = await _cityRepository.Create(city);
        return MapToDto(createdCity);
    }

    public async Task<bool> UpdateCity(int id, CityDto cityDto)
    {
        var city = MapToEntity(cityDto);
        return await _cityRepository.Update(id, city);
    }

    public async Task<bool> DeleteCity(int id)
    {
        return await _cityRepository.Delete(id);
    }

    private static CityDto MapToDto(City city)
    {
        return new CityDto
        {
            Id = city.Id,
            Name = city.Name,
            Latitude = city.Latitude,
            Longitude = city.Longitude,
            Country = city.Country,
            State = city.State
        };
    }

    private static City MapToEntity(CityDto cityDto)
    {
        return new City
        {
            Name = cityDto.Name,
            Latitude = cityDto.Latitude,
            Longitude = cityDto.Longitude,
            Country = cityDto.Country,
            State = cityDto.State
        };
    }
}
