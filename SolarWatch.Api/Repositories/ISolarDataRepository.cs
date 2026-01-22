using SolarWatch.Api.Data;

namespace SolarWatch.Api.Repositories;

public interface ISolarDataRepository : IRepository<Solar, int>
{
    Task<Solar?> ReadByCityAndDate(int cityId, DateOnly date);
}