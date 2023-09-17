using SolarWatch.Models.Cities;
using System.Text.Json.Serialization;

namespace SolarWatch.Models.SunriseSunset
{
    public class SunriseSunsetResults
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public DateTimeOffset Sunrise { get; set; }
        public DateTimeOffset Sunset { get; set; }

        public int CityId { get; set; }

        [JsonIgnore]
        public City City { get; set; }

    }
}
