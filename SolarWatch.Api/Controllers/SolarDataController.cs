using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Api.Data;
using SolarWatch.Api.DTOs;
using SolarWatch.Api.Repositories;

namespace SolarWatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SolarDataController : ControllerBase
{
    private readonly ISolarDataRepository _solarDataRepository;

    public SolarDataController(ISolarDataRepository solarDataRepository)
    {
        _solarDataRepository = solarDataRepository;
    }

    [HttpGet]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<IEnumerable<SolarDto>>> GetAll()
    {
        try
        {
            var solarData = await _solarDataRepository.GetAll();
            var solarDtos = solarData.Select(s => MapToDto(s)).ToList();
            return Ok(solarDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<SolarDto>> GetById([Required] int id)
    {
        try
        {
            var solar = await _solarDataRepository.Read(id);
            if (solar == null)
            {
                return NotFound($"Solar data with id {id} not found.");
            }

            return Ok(MapToDto(solar));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SolarDto>> Create([Required] SolarDto solarDto)
    {
        try
        {
            var solar = MapToEntity(solarDto);
            var createdSolar = await _solarDataRepository.Create(solar);
            return CreatedAtAction(nameof(GetById), new { id = createdSolar.Id }, MapToDto(createdSolar));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([Required] int id, [Required] SolarDto solarDto)
    {
        try
        {
            var solar = MapToEntity(solarDto);
            var success = await _solarDataRepository.Update(id, solar);
            if (!success)
            {
                return NotFound($"Solar data with id {id} not found.");
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
            var success = await _solarDataRepository.Delete(id);
            if (!success)
            {
                return NotFound($"Solar data with id {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
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
