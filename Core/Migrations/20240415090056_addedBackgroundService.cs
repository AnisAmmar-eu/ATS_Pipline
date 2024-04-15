using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class addedBackgroundService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "oldestShooting",
                table: "IOTDevice",
                newName: "WatchdogTime");

            migrationBuilder.AddColumn<string>(
                name: "AnodeType",
                table: "IOTDevice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Family",
                table: "IOTDevice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstanceMatchID",
                table: "IOTDevice",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StationID",
                table: "IOTDevice",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnodeType",
                table: "IOTDevice");

            migrationBuilder.DropColumn(
                name: "Family",
                table: "IOTDevice");

            migrationBuilder.DropColumn(
                name: "InstanceMatchID",
                table: "IOTDevice");

            migrationBuilder.DropColumn(
                name: "StationID",
                table: "IOTDevice");

            migrationBuilder.RenameColumn(
                name: "WatchdogTime",
                table: "IOTDevice",
                newName: "oldestShooting");
        }
    }
}
