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
        private readonly ISunriseSunsetRepository _sunriseSunsetRepository;

        public SolarController(ILogger<SolarController> logger, IWeatherDataProvider weatherDataProvider,
            IJsonProcessor jsonProcessor, ICityRepository cityRepository, ISunriseSunsetRepository sunriseSunsetRepository)
        {
            _logger = logger;
            _weatherDataProvider = weatherDataProvider;
            _jsonProcessor = jsonProcessor;
            _httpClient = new HttpClient();
            _cityRepository = cityRepository;
            _sunriseSunsetRepository = sunriseSunsetRepository;
        }

        [HttpGet]
        [Route("api/solar")]
        public async Task<ActionResult<SunriseSunsetResults>> GetSunriseSunset(string cityName, DateTime date)
        {
            var city = _cityRepository.GetByName(cityName);
            if (city == null)
            {
                var GeoData = await _weatherDataProvider.GetLatLon(cityName);
                var GeoResult = _jsonProcessor.GetGeocodingApiResponse(GeoData);

                var lat = GeoResult.Coord.Lat;
                var lon = GeoResult.Coord.Lon;

                city = new City { Name = cityName, Coordinates = new Coordinates { Lat = lat, Lon = lon } };
                await _cityRepository.AddAsync(city);

                Console.WriteLine("-------------");
                Console.WriteLine($"| City:{city.Name} was created in the DB |");
                Console.WriteLine("-------------");
                Console.WriteLine();

            }

            try
            {
                var sunriseSunset = await _sunriseSunsetRepository.GetByCityAndDateAsync(city.Id, date);
                if (sunriseSunset == null)
                {
                    // Fetch sunrise/sunset data from external API
                    var weatherData = await _weatherDataProvider.GetSunriseSunset(city.Coordinates.Lat, city.Coordinates.Lon, date);
                    var sunriseSunsetData = _jsonProcessor.Process(weatherData, cityName, date);
                    Console.WriteLine($"JSON PROCESS SUCCESS: {sunriseSunsetData.Sunrise}");
                    //await _sunriseSunsetRepository.AddAsync(sunriseSunsetData);

                    Console.WriteLine("-------------");
                    Console.WriteLine($"| City:{sunriseSunsetData.City} was isnerted into the DB |");
                    Console.WriteLine("-------------");
                    Console.WriteLine();
                    return Ok(sunriseSunsetData);
                }

                return Ok(sunriseSunset);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting weather data");
                return NotFound("Error getting weather data");
            }
        }

    }
}
