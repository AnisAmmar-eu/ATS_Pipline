using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class addStationInAnode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "S1S2StationID",
                table: "Anode",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "S3S4StationID",
                table: "Anode",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "S5StationID",
                table: "Anode",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "S1S2StationID",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S3S4StationID",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S5StationID",
                table: "Anode");
        }
    }
}
