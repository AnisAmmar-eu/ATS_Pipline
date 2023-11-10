using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class removedTemperatureFromOTCamera : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "IOTDevice");

            migrationBuilder.RenameColumn(
                name: "CameraRID",
                table: "BITemperature",
                newName: "TemperatureRID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TemperatureRID",
                table: "BITemperature",
                newName: "CameraRID");

            migrationBuilder.AddColumn<double>(
                name: "Temperature",
                table: "IOTDevice",
                type: "float",
                nullable: true);
        }
    }
}
