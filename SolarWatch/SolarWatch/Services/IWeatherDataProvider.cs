namespace SolarWatch.Services
{
    public interface IWeatherDataProvider
    {
        Task<string> GetLatLon(string city);
        Task<string> GetSunriseSunset(double lat, double lon, DateTime date);
    }
}
