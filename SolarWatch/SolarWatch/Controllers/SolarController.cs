using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.DTOs;
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
            _cityRepository = cityRepository;
            _sunriseSunsetRepository = sunriseSunsetRepository;
        }


        [HttpGet("GetByName"), Authorize(Roles = "Admin, User")]
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
            }
            try
            {
                var sunriseSunset = await _sunriseSunsetRepository.GetByCityAndDateAsync(city.Id, date.Date);
                if (sunriseSunset == null)
                {
                    // Fetch sunrise/sunset data from external API
                    var weatherData = await _weatherDataProvider.GetSunriseSunset(city.Coordinates.Lat, city.Coordinates.Lon, date);
                    var sunriseSunsetData = _jsonProcessor.Process(weatherData, cityName, date);
                    sunriseSunsetData.City = city;

                    var newResult = new SunriseSunsetResults
                    {
                        Id = sunriseSunsetData.Id,
                        Sunrise = sunriseSunsetData.Sunrise,
                        Sunset = sunriseSunsetData.Sunset,
                        CityId = city.Id,
                        City = city,
                        Date = date
                    };
                    await _sunriseSunsetRepository.AddAsync(newResult);
                    return Ok(newResult);
                }

                return Ok(sunriseSunset);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting weather data");
                return NotFound("Error getting weather data");
            }
        }

        [HttpPut("UpdateCityData"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCityByName(string cityName, [FromBody] CityUpdateDTO cityUpdateModel)
        {
            var city = _cityRepository.GetByName(cityName);
            City cityToUpdate = new City();
            cityToUpdate = city;

            try
            {
                if (cityToUpdate == null)
                {
                    return NotFound("Error finding city, provide an existing city name");
                }

                await _cityRepository.UpdateAsync(cityToUpdate);

                return Ok("City updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating city");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteByName"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteByName(string cityName)
        {
            var city = _cityRepository.GetByName(cityName);
            if (city == null)
            {
                return NotFound("Error finding city, provide an existing city name");
            }
            try
            {
                await _cityRepository.DeleteAsync(city);
                return Ok("City deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting city");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
