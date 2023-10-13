using Microsoft.AspNetCore.Mvc.Testing;
using SolarWatch.Models.SunriseSunset;
using SolarWatch.Services.Authentication;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SolarWatchTest
{
    public class WeatherProviderTest : WebApplicationFactory<Program>
    {
        private HttpClient _client;
        private JsonSerializerOptions _jsonOptions;
        private AuthResponse _authResponse;

        [SetUp]
        public void Setup()
        {
            string connectionString = "Server=localhost,1433;Database=SolarWatchApi;User Id=sa;Password=yourStrong(!)Password;Encrypt=True;TrustServerCertificate=True;\"";
            Environment.SetEnvironmentVariable("CONNECTION_STRING", connectionString);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _client = CreateClient();

            AuthRequest authRequest = new AuthRequest("gulyasmate21@gmail.com", "123456");
            string jsonString = JsonSerializer.Serialize(authRequest);
            StringContent jsonStringContent = new StringContent(jsonString);
            jsonStringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = _client.PostAsync("/Login", jsonStringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var desContent = JsonSerializer.Deserialize<AuthResponse>(content, options);
            var token = desContent.Token;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        [Test]
        public async Task GetSunriseSunset_ValidRequest_ReturnsSunriseSunset()
        {
            // Arrange
            var cityName = "New York";
            var date = DateTime.Now.Date;

            // Act
            var response = await _client.GetAsync($"/Solar/GetByName?cityName={cityName}&date={date:yyyy-MM-dd}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SunriseSunsetResults>(responseContent, _jsonOptions);

            // Add your specific assertions on the result here
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

    }
}
