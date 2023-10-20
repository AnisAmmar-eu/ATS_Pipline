using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class DetectionColumnsRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TSShooting",
                table: "Packet",
                newName: "ShootingTS");

            migrationBuilder.RenameColumn(
                name: "IsMismatched",
                table: "Packet",
                newName: "IsSameType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShootingTS",
                table: "Packet",
                newName: "TSShooting");

            migrationBuilder.RenameColumn(
                name: "IsSameType",
                table: "Packet",
                newName: "IsMismatched");
        }
    }
}
