using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Station_to_StationID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Station",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "Station",
                table: "AlarmLog");

            migrationBuilder.AddColumn<int>(
                name: "StationID",
                table: "AlarmRT",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StationID",
                table: "AlarmLog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StationID",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "StationID",
                table: "AlarmLog");

            migrationBuilder.AddColumn<string>(
                name: "Station",
                table: "AlarmRT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Station",
                table: "AlarmLog",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
