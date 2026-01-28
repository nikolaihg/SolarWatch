using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Api.DTOs;
using SolarWatch.Api.Services;

namespace SolarWatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SolarWatchController : ControllerBase
{
    private readonly ISunriseSunsetService _sunriseSunsetService;
    private readonly ILocationService _locationService;


    public SolarWatchController(ILocationService locationService, ISunriseSunsetService sunriseSunsetService)
    {
        _locationService = locationService;
        _sunriseSunsetService = sunriseSunsetService;
    }

    [HttpGet]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<SunriseSunsetResult>> Get([Required] string city, DateOnly? date)
    {
        try
        {
            var location = await _locationService.GetCordinates(city);

            var result = !date.HasValue
                ? await _sunriseSunsetService.GetSunriseSunset(location.latitude, location.longitude)
                : await _sunriseSunsetService.GetSunriseSunset(
                    location.latitude,
                    location.longitude,
                    date.Value);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}