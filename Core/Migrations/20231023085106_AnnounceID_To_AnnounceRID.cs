using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AnnounceID_To_AnnounceRID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Announcement_AnnounceID",
                table: "Packet",
                newName: "AnnounceRID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnnounceRID",
                table: "Packet",
                newName: "Announcement_AnnounceID");
        }
    }
}
