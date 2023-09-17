using SolarWatch.Models.SunriseSunset;

namespace SolarWatch.Services.Repositories
{
    public interface ISunriseSunsetRepository
    {
        Task<SunriseSunsetResults> GetByCityAndDateAsync(int cityId, DateTime date);
        Task AddAsync(SunriseSunsetResults sunriseSunset);
        Task<IEnumerable<SunriseSunsetResults>> GetAll();

        Task Delete(SunriseSunsetResults city);
        Task Update(SunriseSunsetResults city);
    }
}
