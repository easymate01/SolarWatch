namespace SolarWatch.Services
{
    public class WeatherProvider : IWeatherDataProvider
    {
        private readonly ILogger<WeatherProvider> _logger;
        private readonly string apiKey = "de4ac831cb404cfba42750c4ad1aceb9";
        public WeatherProvider(ILogger<WeatherProvider> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetLatLon(string city)
        {
            var geoUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

            using var client = new HttpClient();
            _logger.LogInformation("Calling OpenWeather API with url: {url}", geoUrl);

            var response = await client.GetAsync(geoUrl);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetSunriseSunset(double lat, double lon, DateTime date)
        {
            var sunriseSunsetUrl = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date:yyyy-MM-dd}&formatted=0";

            using var client = new HttpClient();
            _logger.LogInformation("Calling OpenWeather API with url: {url}", sunriseSunsetUrl);

            var response = await client.GetAsync(sunriseSunsetUrl);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
