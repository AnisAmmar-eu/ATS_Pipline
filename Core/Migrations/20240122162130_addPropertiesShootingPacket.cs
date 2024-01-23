using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class addPropertiesShootingPacket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Cam01Temp",
                table: "Packet",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Cam02Temp",
                table: "Packet",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TT01",
                table: "Packet",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cam01Temp",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Cam02Temp",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "TT01",
                table: "Packet");
        }
    }
}
