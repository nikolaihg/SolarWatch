using Microsoft.AspNetCore.Mvc;
using Moq;
using SolarWatch.Api.Controllers;
using SolarWatch.Api.DTOs;
using SolarWatch.Api.Services;

namespace SolarWatch.Test;

[TestFixture]
public class SolarWatchControllerTest
{
    private Mock<ILocationService> _locationServiceMock;
    private Mock<ISunriseSunsetService> _sunriseSunsetServiceMock;
    private SolarWatchController _controller;

    [SetUp]
    public void Setup()
    {
        _locationServiceMock = new Mock<ILocationService>();
        _sunriseSunsetServiceMock = new Mock<ISunriseSunsetService>();
        _controller = new SolarWatchController(_locationServiceMock.Object, _sunriseSunsetServiceMock.Object);
    }

    // Invalid date formatting
    [Test]
    public void GetWithInvalidDateFormatting()
    {
        // Arrange
        var city = "Bergen";
        var badDate = new DateOnly(1, 1, 1);
        var coords = new LatitudeLongitudeResult(60.3943055, 5.3259192);
        // Act
        _locationServiceMock.Setup(x => x.GetCordinates(city)).ReturnsAsync(coords);
        _sunriseSunsetServiceMock.Setup(x => x.GetSunriseSunset(coords.latitude, coords.longitude, badDate))
            .ThrowsAsync(new FormatException("Invalid Date"));
        // Assert
        Assert.ThrowsAsync<FormatException>(async () => await _controller.Get(city, badDate));
    }

    // City name does not exist
    [Test]
    public async Task GetWithInvalidCityReturnsNotFound()
    {
        // Arrange
        var city = "thiscitydoesnotexist";
        // Act
        _locationServiceMock.Setup(x => x.GetCordinates(city)).ThrowsAsync(new KeyNotFoundException("City not found"));
        // Assert
        var result = await _controller.Get(city, null);
        var notFound = result.Result as NotFoundObjectResult;
        Assert.That(notFound, Is.Not.Null);
        Assert.That(notFound.StatusCode, Is.EqualTo(404));
    }

    // Correct Get Request 
    [Test]
    public async Task GetWithoutDateReturnsOkWithResult()
    {
        // Arrange
        var city = "Bergen";
        var coords = new LatitudeLongitudeResult(60.3943055, 5.3259192);
        var expected = new SunriseSunsetResult("8:15:34 AM", "3:24:24 PM");
        // Act
        _locationServiceMock.Setup(x => x.GetCordinates(city)).ReturnsAsync(coords);
        _sunriseSunsetServiceMock.Setup(x => x.GetSunriseSunset(coords.latitude, coords.longitude))
            .ReturnsAsync(expected);
        // Assert
        var result = await _controller.Get(city, null);
        var ok = result.Result as OkObjectResult;
        var value = ok!.Value as SunriseSunsetResult;
        Assert.That(value, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(value!.Sunrise, Is.EqualTo(expected.Sunrise));
            Assert.That(value.Sunset, Is.EqualTo(expected.Sunset));
        });
    }

    [Test]
    public async Task GetWithDateReturnsOkWithResult()
    {
        // Arrange
        var city = "Bergen";
        var coords = new LatitudeLongitudeResult(60.3943055, 5.3259192);
        var date = new DateOnly(2025, 12, 23);
        var expected = new SunriseSunsetResult("8:42:02 AM", "2:33:41 PM");
        // Act
        _locationServiceMock.Setup(x => x.GetCordinates(city)).ReturnsAsync(coords);
        _sunriseSunsetServiceMock.Setup(x => x.GetSunriseSunset(coords.latitude, coords.longitude, date))
            .ReturnsAsync(expected);
        // Assert
        var result = await _controller.Get(city, date);
        var ok = result.Result as OkObjectResult;
        var value = ok!.Value as SunriseSunsetResult;
        Assert.That(value, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(value!.Sunrise, Is.EqualTo(expected.Sunrise));
            Assert.That(value.Sunset, Is.EqualTo(expected.Sunset));
        });
    }
}