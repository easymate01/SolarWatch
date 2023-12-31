﻿namespace SolarWatch.Services
{
    public class WeatherProvider : IWeatherDataProvider
    {
        private readonly ILogger<WeatherProvider> _logger;
        private readonly string apiKey = "81801e606480c0782a26ad943cc4a746";
        public WeatherProvider(ILogger<WeatherProvider> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetLatLon(string city)
        {
            var geoUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

            using var client = new HttpClient();
            _logger.LogInformation("Gettung GetLatLon with: OpenWeather API with url: {url}", geoUrl);

            var response = await client.GetAsync(geoUrl);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetSunriseSunset(double lat, double lon, DateTime date)
        {
            var sunriseSunsetUrl = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date:yyyy-MM-dd}&formatted=0";

            using var client = new HttpClient();
            _logger.LogInformation("Getting SunriseSunset with: OpenWeather API with url: {url}", sunriseSunsetUrl);

            var response = await client.GetAsync(sunriseSunsetUrl);
            _logger.LogInformation("Getting SunriseSunset succed... ");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
