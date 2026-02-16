using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Api.DTOs;
using SolarWatch.Api.Models;
using SolarWatch.Api.Repositories;

namespace SolarWatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CityController : ControllerBase
{
    private readonly ICityRepository _cityRepository;

    public CityController(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    [HttpGet]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetAll()
    {
        try
        {
            var cities = await _cityRepository.GetAll();
            var cityDtos = cities.Select(c => MapToDto(c)).ToList();
            return Ok(cityDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("names")]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<IEnumerable<CityNameDto>>> GetAllNames()
    {
        try
        {
            var cities = await _cityRepository.GetAll();
            var cityNameDtos = cities.Select(c => new CityNameDto { Name = c.Name.ToLower() }).ToList();
            return Ok(cityNameDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<CityDto>> GetById([Required] int id)
    {
        try
        {
            var city = await _cityRepository.Read(id);
            if (city == null)
            {
                return NotFound($"City with id {id} not found.");
            }

            return Ok(MapToDto(city));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CityDto>> Create([Required] CityDto cityDto)
    {
        try
        {
            var city = MapToEntity(cityDto);
            var createdCity = await _cityRepository.Create(city);
            return CreatedAtAction(nameof(GetById), new { id = createdCity.Id }, MapToDto(createdCity));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([Required] int id, [Required] CityDto cityDto)
    {
        try
        {
            var city = MapToEntity(cityDto);
            var success = await _cityRepository.Update(id, city);
            if (!success)
            {
                return NotFound($"City with id {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([Required] int id)
    {
        try
        {
            var success = await _cityRepository.Delete(id);
            if (!success)
            {
                return NotFound($"City with id {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
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