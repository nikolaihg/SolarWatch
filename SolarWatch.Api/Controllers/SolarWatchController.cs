using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Api.DTOs;
using SolarWatch.Api.Services;

namespace SolarWatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<SunriseSunsetResult>> Get([Required] string city, DateOnly? date)
    {
        var location = await _locationService.GetCordinates(city);
        if (!date.HasValue)
        {
            var result =
                await _sunriseSunsetService.GetSunriseSunset(location.latitude, location.longitude);
            return Ok(result);
        }
        else
        {
            var result = await _sunriseSunsetService.GetSunriseSunset(location.latitude, location.longitude, date.Value);
            return Ok(result);
        }
    }
}