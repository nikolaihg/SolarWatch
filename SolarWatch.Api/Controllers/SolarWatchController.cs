using System.Text.Json;
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
    private readonly ILogger<SolarWatchController> _logger;


    public SolarWatchController(ILogger<SolarWatchController> logger, ILocationService locationService,
        ISunriseSunsetService sunriseSunsetService, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _locationService = locationService;
        _sunriseSunsetService = sunriseSunsetService;
    }


    [HttpGet]
    public async Task<ActionResult<SunriseSunsetResult>> Get(string city)
    {
        var location = await _locationService.GetCordinates(city);
        var result = await _sunriseSunsetService.GetSunriseSunset(location.latitude, location.longitude);
        return Ok(result);
    }
}