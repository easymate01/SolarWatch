using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarWatch.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSSCityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SunriseSunsetTimes_CityId",
                table: "SunriseSunsetTimes");

            migrationBuilder.CreateIndex(
                name: "IX_SunriseSunsetTimes_CityId",
                table: "SunriseSunsetTimes",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SunriseSunsetTimes_CityId",
                table: "SunriseSunsetTimes");

            migrationBuilder.CreateIndex(
                name: "IX_SunriseSunsetTimes_CityId",
                table: "SunriseSunsetTimes",
                column: "CityId",
                unique: true);
        }
    }
}
