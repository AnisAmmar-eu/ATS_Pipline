using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Packet_FieldToStationCycle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InFurnace_StationCycleID",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StationCycleID",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packet_InFurnace_StationCycleID",
                table: "Packet",
                column: "InFurnace_StationCycleID");

            migrationBuilder.CreateIndex(
                name: "IX_Packet_StationCycleID",
                table: "Packet",
                column: "StationCycleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Packet_StationCycle_InFurnace_StationCycleID",
                table: "Packet",
                column: "InFurnace_StationCycleID",
                principalTable: "StationCycle",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Packet_StationCycle_StationCycleID",
                table: "Packet",
                column: "StationCycleID",
                principalTable: "StationCycle",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packet_StationCycle_InFurnace_StationCycleID",
                table: "Packet");

            migrationBuilder.DropForeignKey(
                name: "FK_Packet_StationCycle_StationCycleID",
                table: "Packet");

            migrationBuilder.DropIndex(
                name: "IX_Packet_InFurnace_StationCycleID",
                table: "Packet");

            migrationBuilder.DropIndex(
                name: "IX_Packet_StationCycleID",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "InFurnace_StationCycleID",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "StationCycleID",
                table: "Packet");
        }
    }
}
