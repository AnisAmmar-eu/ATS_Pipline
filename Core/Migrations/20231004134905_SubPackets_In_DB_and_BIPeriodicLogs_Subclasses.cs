using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class SubPackets_In_DB_and_BIPeriodicLogs_Subclasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BIPeriodicLogs",
                table: "BIPeriodicLogs");

            migrationBuilder.RenameTable(
                name: "BIPeriodicLogs",
                newName: "BIPeriodicLog");

            migrationBuilder.AddColumn<int>(
                name: "AnodeIDKey",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnodePosition",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnodeType",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BakedPosition",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FTAPickUp",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FTASuckPit",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FTAinPIT",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GlobalStationStatus",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GreenPosition",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDoubleAnode",
                table: "Packet",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LedStatus",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginID",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PITHeight",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PITNumber",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PITSectionNumber",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PalletSide",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcedurePerformance",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ShootingTS",
                table: "Packet",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TSCentralConveyor",
                table: "Packet",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TSLoad",
                table: "Packet",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TSUnpackPIT",
                table: "Packet",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrolleyNumber",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "BIPeriodicLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BIPeriodicLog",
                table: "BIPeriodicLog",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BIPeriodicLog",
                table: "BIPeriodicLog");

            migrationBuilder.DropColumn(
                name: "AnodeIDKey",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "AnodePosition",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "AnodeType",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "BakedPosition",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "FTAPickUp",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "FTASuckPit",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "FTAinPIT",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "GlobalStationStatus",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "GreenPosition",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "IsDoubleAnode",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "LedStatus",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "OriginID",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "PITHeight",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "PITNumber",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "PITSectionNumber",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "PalletSide",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "ProcedurePerformance",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "ShootingTS",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "TSCentralConveyor",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "TSLoad",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "TSUnpackPIT",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "TrolleyNumber",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "BIPeriodicLog");

            migrationBuilder.RenameTable(
                name: "BIPeriodicLog",
                newName: "BIPeriodicLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BIPeriodicLogs",
                table: "BIPeriodicLogs",
                column: "ID");
        }
    }
}
