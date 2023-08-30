using SolarWatch.Models.SunriseSunset;

namespace SolarWatch.Models.Cities
{
    public class City
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public Coordinates Coordinates { get; set; }

        public SunriseSunsetResults SunriseSunsetInfo { get; set; }

    }
}
