using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SolarWatch.Contracts;
using SolarWatch.Models.SunriseSunset;
using System.Net;
using System.Net.Http.Json;

namespace SolarWatchTest
{
    [TestFixture]
    public class IntegrationTest : WebApplicationFactory<Program>
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void Setup()
        {
            _client = CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
        }


        [Test]
        public async Task UserRegistrationIntegrationTest()
        {
            // Arrange: Create a new user registration request DTO
            var registrationRequest = new RegistrationRequest("testuser", "testuser@example.com", "P@ssw0rd");


            // Act: Send a POST request to the user registration endpoint
            var response = await _client.PostAsJsonAsync("/Auth/Register", registrationRequest);

            // Assert: Check if the response indicates success (status code 200 OK)
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        [Test]
        public async Task TestGetByNameEndpoint_ExistingCity_ShouldReturnOk()
        {
            var cityName = "London";
            var date = DateTime.Now.Date;

            var response = await _client.GetAsync($"/Solar/GetByName?cityName={cityName}&date =${date}");

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SunriseSunsetApiResponse>(responseContent);

            Assert.IsNotNull(result);
            Assert.AreEqual(cityName, result.Results.City.Name);

        }

        [Test]
        public async Task TestGetByNameEndpoint_NonExistingCity_ShouldReturnNotFound()
        {
            var cityName = "aaaaaaaaaaa";

            var response = await _client.GetAsync($"/SolarWatch/GetByName?cityName={cityName}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Error getting weather data", responseContent);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            // Dispose of the HttpClient in case it's needed
            _client.Dispose();
        }

    }
}
