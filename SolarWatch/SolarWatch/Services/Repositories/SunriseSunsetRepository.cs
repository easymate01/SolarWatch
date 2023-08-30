using Microsoft.EntityFrameworkCore;
using SolarWatch.Models.SunriseSunset;

namespace SolarWatch.Services.Repositories
{
    public class SunriseSunsetRepository : ISunriseSunsetRepository
    {



        public async Task<SunriseSunsetResults> GetByCityAndDateAsync(int cityId, DateTime date)
        {
            using var dbContext = new SolarWatchApiContext();
            return await dbContext.SunriseSunsetTimes
                .SingleOrDefaultAsync(ss => ss.CityId == cityId && ss.Date == date);
        }

        public async Task AddAsync(SunriseSunsetResults sunriseSunset)
        {
            using var dbContext = new SolarWatchApiContext();
            dbContext.Add(sunriseSunset);
            await dbContext.SaveChangesAsync();
        }
    }
}
