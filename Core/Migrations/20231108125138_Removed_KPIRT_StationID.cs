using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Removed_KPIRT_StationID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StationID",
                table: "KPIRT");

            migrationBuilder.DropColumn(
                name: "StationID",
                table: "KPILog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StationID",
                table: "KPIRT",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StationID",
                table: "KPILog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
