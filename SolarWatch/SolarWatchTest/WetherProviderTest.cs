using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Models.SunriseSunset;
using SolarWatch.Services;
using SolarWatch.Services.Json;

namespace SolarWatchTest
{
    [TestFixture]
    public class WetherProviderTest
    {
        private Mock<ILogger<SolarController>> _loggerMock;
        private Mock<IWeatherDataProvider> _weatherDataProviderMock;
        private Mock<IJsonProcessor> _jsonProcessorMock;
        private SolarController _controller;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<SolarController>>();
            _weatherDataProviderMock = new Mock<IWeatherDataProvider>();
            _jsonProcessorMock = new Mock<IJsonProcessor>();
            _controller = new SolarController(_loggerMock.Object, _weatherDataProviderMock.Object, _jsonProcessorMock.Object);
        }

        [Test]
        public async Task GetLatLonReturnsNotFoundResultIfWeatherDataProviderFails()
        {
            var weatherData = "{}";
            _weatherDataProviderMock.Setup(x => x.GetLatLon(It.IsAny<string>())).ThrowsAsync(new Exception());


            var result = await _controller.GetSunriseSunset(It.IsAny<string>(), It.IsAny<DateTime>());

            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }

        [Test]
        public async Task GetSunriseSunsetReturnsNotFoundResultIfWeatherDataProviderFails()
        {
            var weatherData = "{}";
            _weatherDataProviderMock.Setup(x =>
                    x.GetSunriseSunset(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>()))
                    .ThrowsAsync(new Exception());


            var result = await _controller.GetSunriseSunset(It.IsAny<string>(), It.IsAny<DateTime>());

            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }



        [Test]
        public async Task GetLatLontReturnsNotFoundResultIfWeatherDataIsInvalid()
        {
            // Arrange
            var expectedResponse = new GeocodingApiResponse();
            var weatherData = "{}";
            _weatherDataProviderMock.Setup(x => x.GetLatLon(It.IsAny<string>())).ReturnsAsync(weatherData);
            _jsonProcessorMock.Setup(x => x.GetGeocodingApiResponse(weatherData)).Returns(expectedResponse);

            // Act
            var result = await _controller.GetSunriseSunset(It.IsAny<string>(), It.IsAny<DateTime>());

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }

        [Test]
        public async Task GetSunriseSunsettReturnsNotFoundResultIfWeatherDataIsInvalid()
        {
            // Arrange
            var expectedResponse = new SunriseSunsetResults();
            var weatherData = "{}";

            _weatherDataProviderMock.Setup(x =>
                x.GetSunriseSunset(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>())).ReturnsAsync(weatherData);

            _jsonProcessorMock.Setup(x => x.Process(weatherData, It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedResponse);

            // Act
            var result = await _controller.GetSunriseSunset(It.IsAny<string>(), It.IsAny<DateTime>());

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }
    }
}