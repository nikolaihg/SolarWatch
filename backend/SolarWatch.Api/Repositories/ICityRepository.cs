using SolarWatch.Api.Models;

namespace SolarWatch.Api.Repositories;

public interface ICityRepository : IRepository<City, int>
{
    Task<City?> ReadByName(string name);
    Task<City?> ReadByCoordinates(double latitude, double longitude);
}