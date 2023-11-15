using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AddedSigningAndMatchingCycles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "S3S4Cycle_MatchingCamera1",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "S3S4Cycle_MatchingCamera2",
                table: "StationCycle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
