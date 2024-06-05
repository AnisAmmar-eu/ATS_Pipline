using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class ToNotifyNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "ToNotify",
                newName: "ShootingTS");

            migrationBuilder.RenameColumn(
                name: "Station",
                table: "ToNotify",
                newName: "StationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StationID",
                table: "ToNotify",
                newName: "Station");

            migrationBuilder.RenameColumn(
                name: "ShootingTS",
                table: "ToNotify",
                newName: "Timestamp");
        }
    }
}
