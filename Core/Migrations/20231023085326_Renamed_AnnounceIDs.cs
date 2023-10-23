using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Renamed_AnnounceIDs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InFurnace_AnnounceID",
                table: "Packet",
                newName: "OutAnnounceID");

            migrationBuilder.RenameColumn(
                name: "AnnounceRID",
                table: "Packet",
                newName: "InAnnounceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OutAnnounceID",
                table: "Packet",
                newName: "InFurnace_AnnounceID");

            migrationBuilder.RenameColumn(
                name: "InAnnounceID",
                table: "Packet",
                newName: "AnnounceRID");
        }
    }
}
