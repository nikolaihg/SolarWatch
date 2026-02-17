using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Api.DTOs;
using SolarWatch.Api.Services;

namespace SolarWatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpGet]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetAll()
    {
        try
        {
            var cityDtos = await _cityService.GetAllCities();
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
            var cityNameDtos = await _cityService.GetAllCityNames();
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
            var cityDto = await _cityService.GetCityById(id);
            if (cityDto == null)
            {
                return NotFound($"City with id {id} not found.");
            }

            return Ok(cityDto);
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
            var createdCity = await _cityService.CreateCity(cityDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCity.Id }, createdCity);
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
            var success = await _cityService.UpdateCity(id, cityDto);
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
            var success = await _cityService.DeleteCity(id);
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
}