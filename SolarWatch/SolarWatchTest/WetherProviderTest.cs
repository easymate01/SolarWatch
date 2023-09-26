using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Models.Cities;
using SolarWatch.Models.SunriseSunset;
using SolarWatch.Services;
using SolarWatch.Services.Json;
using SolarWatch.Services.Repositories;

namespace SolarWatchTest
{
    [TestFixture]
    public class WetherProviderTest
    {
        private Mock<ILogger<SolarController>> _loggerMock;
        private Mock<IWeatherDataProvider> _weatherDataProviderMock;
        private Mock<IJsonProcessor> _jsonProcessorMock;
        private Mock<ICityRepository> _cityRepository;
        private Mock<ISunriseSunsetRepository> _sunriseSunsetRepository;
        private SolarController _controller;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<SolarController>>();
            _weatherDataProviderMock = new Mock<IWeatherDataProvider>();
            _jsonProcessorMock = new Mock<IJsonProcessor>();
            _cityRepository = new Mock<ICityRepository>();
            _sunriseSunsetRepository = new Mock<ISunriseSunsetRepository>();
            _controller = new SolarController(_loggerMock.Object, _weatherDataProviderMock.Object, _jsonProcessorMock.Object,
                _cityRepository.Object, _sunriseSunsetRepository.Object);
        }


        [Test]
        public async Task GetSunriseSunset_CityExists_ReturnsOkResult()
        {
            // Arrange
            var cityName = "ExistingCity";
            var date = DateTime.Now.Date;
            var existingCity = new City { Name = cityName }; // Create an existing city
            var sunriseSunsetData = new SunriseSunsetResults { /* Initialize with data */ };

            _cityRepository.Setup(x => x.GetByName(cityName)).Returns(existingCity);
            _sunriseSunsetRepository.Setup(x => x.GetByCityAndDateAsync(existingCity.Id, date.Date))
                .ReturnsAsync(sunriseSunsetData);

            // Act
            var result = await _controller.GetSunriseSunset(cityName, date);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.AreEqual(sunriseSunsetData, okResult.Value); // Ensure the expected data is returned
        }

        [Test]
        public async Task GetSunriseSunset_CityExists_SunriseSunsetDataMissing_ExternalAPICallFails_ReturnsNotFound()
        {
            // Arrange
            var cityName = "ExistingCity";
            var date = DateTime.Now.Date;
            var existingCity = new City { Name = cityName };

            _cityRepository.Setup(x => x.GetByName(cityName)).Returns(existingCity);
            _sunriseSunsetRepository.Setup(x => x.GetByCityAndDateAsync(existingCity.Id, date.Date)).ReturnsAsync((SunriseSunsetResults)null);
            _weatherDataProviderMock.Setup(x => x.GetSunriseSunset(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>())).ThrowsAsync(new Exception()); // Simulate an API call failure

            // Act
            var result = await _controller.GetSunriseSunset(cityName, date);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }


        //[Test]
        //public async Task GetSunriseSunset_ReturnsOkResult_WhenWeatherDataIsValid()
        //{
        //    // Arrange
        //    var cityName = "Budapest";
        //    var date = DateTime.Now.Date;
        //    var expectedSunriseSunset = new SunriseSunsetResults { /* Initialize with valid data */ };
        //    var weatherData = "{}";

        //    _cityRepository.Setup(x => x.GetByName(cityName)).Returns(new City { Name = cityName });
        //    _sunriseSunsetRepository.Setup(x => x.GetByCityAndDateAsync(It.IsAny<int>(), date.Date))
        //        .ReturnsAsync((SunriseSunsetResults)null); // Simulate sunrise and sunset data missing in repository
        //    _weatherDataProviderMock.Setup(x => x.GetLatLon(cityName)).ReturnsAsync(weatherData);
        //    _jsonProcessorMock.Setup(x => x.GetGeocodingApiResponse(weatherData))
        //        .Returns(new GeocodingApiResponse { Coord = new Coordinates { Lat = 12.34, Lon = 56.78 } });
        //    _weatherDataProviderMock.Setup(x => x.GetSunriseSunset(12.34, 56.78, date))
        //        .ReturnsAsync("{}"); // Simulate valid weather data
        //    _jsonProcessorMock.Setup(x => x.Process("{}", cityName, date)).Returns(expectedSunriseSunset);

        //    // Act
        //    var result = await _controller.GetSunriseSunset(cityName, date);


        //    // Assert
        //    Assert.IsInstanceOf<OkObjectResult>(result.Result);
        //    var okResult = (OkObjectResult)result.Result;
        //    Assert.AreEqual(expectedSunriseSunset, okResult.Value);
        //}




    }
}