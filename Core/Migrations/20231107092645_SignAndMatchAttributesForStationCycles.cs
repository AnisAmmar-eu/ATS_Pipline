using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class SignAndMatchAttributesForStationCycles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchingCamera1",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MatchingCamera2",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "S3S4Cycle_MatchingCamera1",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "S3S4Cycle_MatchingCamera2",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SignStatus1",
                table: "StationCycle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SignStatus2",
                table: "StationCycle",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchingCamera1",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "MatchingCamera2",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "S3S4Cycle_MatchingCamera1",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "S3S4Cycle_MatchingCamera2",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "SignStatus1",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "SignStatus2",
                table: "StationCycle");
        }
    }
}
