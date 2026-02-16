using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Api.DTOs;
using SolarWatch.Api.Services;

namespace SolarWatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SolarDataController : ControllerBase
{
    private readonly ISolarDataService _solarDataService;

    public SolarDataController(ISolarDataService solarDataService)
    {
        _solarDataService = solarDataService;
    }

    [HttpGet]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<IEnumerable<SolarDto>>> GetAll()
    {
        try
        {
            var solarDtos = await _solarDataService.GetAllSolarData();
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
            var solarDto = await _solarDataService.GetSolarDataById(id);
            if (solarDto == null)
            {
                return NotFound($"Solar data with id {id} not found.");
            }

            return Ok(solarDto);
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
            var createdSolar = await _solarDataService.CreateSolarData(solarDto);
            return CreatedAtAction(nameof(GetById), new { id = createdSolar.Id }, createdSolar);
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
            var success = await _solarDataService.UpdateSolarData(id, solarDto);
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
            var success = await _solarDataService.DeleteSolarData(id);
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
}