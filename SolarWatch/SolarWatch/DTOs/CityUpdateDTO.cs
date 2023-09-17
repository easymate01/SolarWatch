using SolarWatch.Models;

namespace SolarWatch.DTOs
{
    public class CityUpdateDTO
    {
        public string? CityName { get; set; } = null;

        public Coordinates? Coordinates { get; set; } = null;

    }
}
