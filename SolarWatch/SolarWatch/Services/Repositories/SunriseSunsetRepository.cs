using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Models.SunriseSunset;

namespace SolarWatch.Services.Repositories
{
    public class SunriseSunsetRepository : ISunriseSunsetRepository
    {
        public async Task<IEnumerable<SunriseSunsetResults>> GetAll()
        {
            using var dbContext = new SolarWatchApiContext();
            return await dbContext.SunriseSunsetTimes.ToListAsync();
        }
        public async Task<SunriseSunsetResults> GetByCityAndDateAsync(int cityId, DateTime date)
        {
            using var dbContext = new SolarWatchApiContext();
            Console.WriteLine($"Getting sunrise and sunset data for CityId: {cityId} and Date: {date}");

            return await dbContext.SunriseSunsetTimes
                .SingleOrDefaultAsync(ss => ss.CityId == cityId && ss.Date == date);
        }

        public async Task AddAsync(SunriseSunsetResults sunriseSunset)
        {
            try
            {
                using var dbContext = new SolarWatchApiContext();
                dbContext.SunriseSunsetTimes.Add(sunriseSunset);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and take appropriate action
                Console.WriteLine($"Error adding sunrise/sunset data: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        public async Task Delete(SunriseSunsetResults sunrise)
        {
            using var dbContext = new SolarWatchApiContext();
            dbContext.Remove(sunrise);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(SunriseSunsetResults sunrise)
        {
            using var dbContext = new SolarWatchApiContext();
            dbContext.Update(sunrise);
            await dbContext.SaveChangesAsync();
        }
    }
}
