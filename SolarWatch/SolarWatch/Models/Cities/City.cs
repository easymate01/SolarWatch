using SolarWatch.Models.SunriseSunset;
using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Models.Cities
{
    public class City
    {
        [Key]
        public int Id { get; init; }
        public string Name { get; init; }

        public Coordinates Coordinates { get; set; }

        public SunriseSunsetResults SunriseSunsetInfo { get; set; }

    }
}
