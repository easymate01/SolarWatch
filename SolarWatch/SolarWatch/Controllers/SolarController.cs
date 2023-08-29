using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models.SunriseSunset;
using SolarWatch.Services;
using SolarWatch.Services.Json;


namespace SolarWatch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolarController : Controller
    {
        private readonly ILogger<SolarController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IWeatherDataProvider _weatherDataProvider;
        private readonly IJsonProcessor _jsonProcessor;

        public SolarController(ILogger<SolarController> logger, IWeatherDataProvider weatherDataProvider,
            IJsonProcessor jsonProcessor)
        {
            _logger = logger;
            _weatherDataProvider = weatherDataProvider;
            _jsonProcessor = jsonProcessor;
            _httpClient = new HttpClient();
        }


        [HttpGet]
        [Route("api/solar")]
        public async Task<ActionResult<SunriseSunsetResults>> GetSunriseSunset(string city, DateTime date)
        {
            try
            {
                var GeoData = await _weatherDataProvider.GetLatLon(city);
                var GeoResult = _jsonProcessor.GetGeocodingApiResponse(GeoData);

                var lat = GeoResult.Coord.Lat;
                var lon = GeoResult.Coord.Lon;

                var weatherData = await _weatherDataProvider.GetSunriseSunset(lat, lon, date);

                return Ok(_jsonProcessor.Process(weatherData, city, date));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting weather data");
                return NotFound("Error getting weather data");
            }
        }
    }
}
