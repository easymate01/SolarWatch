using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Models.Cities;
using SolarWatch.Models.SunriseSunset;
using SolarWatch.Services;
using SolarWatch.Services.Json;
using SolarWatch.Services.Repositories;


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
        private readonly ICityRepository _cityRepository;

        public SolarController(ILogger<SolarController> logger, IWeatherDataProvider weatherDataProvider,
            IJsonProcessor jsonProcessor, ICityRepository cityRepository)
        {
            _logger = logger;
            _weatherDataProvider = weatherDataProvider;
            _jsonProcessor = jsonProcessor;
            _httpClient = new HttpClient();
            _cityRepository = cityRepository;
        }

        [HttpGet]
        [Route("api/solar")]
        public async Task<ActionResult<SunriseSunsetResults>> GetSunriseSunset(string cityName, DateTime date)
        {
            var city = _cityRepository.GetByName(cityName);
            if (city == null)
            {
                Console.WriteLine($"|City:{cityName} was not found in the DB|");
                var GeoData = await _weatherDataProvider.GetLatLon(cityName);
                var GeoResult = _jsonProcessor.GetGeocodingApiResponse(GeoData);

                var lat = GeoResult.Coord.Lat;
                var lon = GeoResult.Coord.Lon;

                city = new City { Name = cityName, Coordinates = new Coordinates { Lat = lat, Lon = lon } };
                await _cityRepository.AddAsync(city);
            }

            try
            {
                var weatherData = await _weatherDataProvider.GetSunriseSunset(city.Coordinates.Lat, city.Coordinates.Lon, date);
                return Ok(_jsonProcessor.Process(weatherData, cityName, date));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting weather data");
                return NotFound("Error getting weather data");
            }
        }

    }
}
