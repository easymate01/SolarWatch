using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarWatch.Migrations
{
    /// <inheritdoc />
    public partial class FixCoordinatesColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "SunriseSunsetTimes");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Cities",
                newName: "Coordinates_Lon");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Cities",
                newName: "Coordinates_Lat");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "SunriseSunsetTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SunriseSunsetTimes_CityId",
                table: "SunriseSunsetTimes",
                column: "CityId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name",
                table: "Cities",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SunriseSunsetTimes_Cities_CityId",
                table: "SunriseSunsetTimes",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SunriseSunsetTimes_Cities_CityId",
                table: "SunriseSunsetTimes");

            migrationBuilder.DropIndex(
                name: "IX_SunriseSunsetTimes_CityId",
                table: "SunriseSunsetTimes");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Name",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "SunriseSunsetTimes");

            migrationBuilder.RenameColumn(
                name: "Coordinates_Lon",
                table: "Cities",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Coordinates_Lat",
                table: "Cities",
                newName: "Latitude");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "SunriseSunsetTimes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
