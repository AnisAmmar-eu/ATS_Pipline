using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class ShootingPacketUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationCycle_Packet_DetectionID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_DetectionID",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "DetectionID",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "DetectionStatus",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "GlobalStationStatus",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "LedStatus",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "MeasuredType",
                table: "Packet");

            migrationBuilder.RenameColumn(
                name: "ProcedurePerformance",
                table: "Packet",
                newName: "SyncIndex");

            migrationBuilder.RenameColumn(
                name: "IsSameType",
                table: "Packet",
                newName: "HasSecondShoot");

            migrationBuilder.RenameColumn(
                name: "AnodeIDKey",
                table: "Packet",
                newName: "Cam02Status");

            migrationBuilder.AddColumn<int>(
                name: "Cam01Status",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasFirstShoot",
                table: "Packet",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cam01Status",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "HasFirstShoot",
                table: "Packet");

            migrationBuilder.RenameColumn(
                name: "SyncIndex",
                table: "Packet",
                newName: "ProcedurePerformance");

            migrationBuilder.RenameColumn(
                name: "HasSecondShoot",
                table: "Packet",
                newName: "IsSameType");

            migrationBuilder.RenameColumn(
                name: "Cam02Status",
                table: "Packet",
                newName: "AnodeIDKey");

            migrationBuilder.AddColumn<int>(
                name: "DetectionID",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DetectionStatus",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GlobalStationStatus",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LedStatus",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeasuredType",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_DetectionID",
                table: "StationCycle",
                column: "DetectionID",
                unique: true,
                filter: "[DetectionID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_StationCycle_Packet_DetectionID",
                table: "StationCycle",
                column: "DetectionID",
                principalTable: "Packet",
                principalColumn: "ID");
        }
    }
}
