using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class NewPacketStructs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnounceID",
                table: "Packet");

            migrationBuilder.AddColumn<bool>(
                name: "IsDouble",
                table: "Packet",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shooting_SyncIndex",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TwinCatStatus",
                table: "Packet",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDouble",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Shooting_SyncIndex",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "TwinCatStatus",
                table: "Packet");

            migrationBuilder.AddColumn<string>(
                name: "AnnounceID",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
