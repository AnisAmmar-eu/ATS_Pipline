using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class S3S4Cycle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "AnnounceID",
                table: "StationCycle",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "StationCycle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "InFurnaceID",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InFurnaceStatus",
                table: "StationCycle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutFurnaceID",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutFurnaceStatus",
                table: "StationCycle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_InFurnaceID",
                table: "StationCycle",
                column: "InFurnaceID",
                unique: true,
                filter: "[InFurnaceID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_OutFurnaceID",
                table: "StationCycle",
                column: "OutFurnaceID",
                unique: true,
                filter: "[OutFurnaceID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_StationCycle_Packet_InFurnaceID",
                table: "StationCycle",
                column: "InFurnaceID",
                principalTable: "Packet",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_StationCycle_Packet_OutFurnaceID",
                table: "StationCycle",
                column: "OutFurnaceID",
                principalTable: "Packet",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationCycle_Packet_InFurnaceID",
                table: "StationCycle");

            migrationBuilder.DropForeignKey(
                name: "FK_StationCycle_Packet_OutFurnaceID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_InFurnaceID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_OutFurnaceID",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "InFurnaceID",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "InFurnaceStatus",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "OutFurnaceID",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "OutFurnaceStatus",
                table: "StationCycle");

            migrationBuilder.AlterColumn<int>(
                name: "AnnounceID",
                table: "StationCycle",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SerialNumber",
                table: "Packet",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
