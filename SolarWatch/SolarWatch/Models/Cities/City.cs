using SolarWatch.Models.SunriseSunset;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarWatch.Models.Cities
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }
        public string Name { get; init; }

        public Coordinates Coordinates { get; set; }

        public SunriseSunsetResults SunriseSunsetInfo { get; set; }

    }
}
