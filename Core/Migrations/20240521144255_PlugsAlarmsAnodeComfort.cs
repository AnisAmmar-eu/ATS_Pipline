using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class PlugsAlarmsAnodeComfort : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPlug",
                table: "ToSign",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NbActiveAlarms",
                table: "ToSign",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasPlug",
                table: "ToMatch",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NbActiveAlarms",
                table: "ToMatch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasPlug",
                table: "StationCycle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NbActiveAlarms",
                table: "StationCycle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasPlug",
                table: "Packet",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NbActiveAlarms",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "S1S2SignStatus1",
                table: "Anode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "S1S2SignStatus2",
                table: "Anode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "S1S2TSFirstShooting",
                table: "Anode",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "S3S4MatchingCamera2",
                table: "Anode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "S3S4SignStatus1",
                table: "Anode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "S3S4SignStatus2",
                table: "Anode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "S3S4TSFirstShooting",
                table: "Anode",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "S5MatchingCamera1",
                table: "Anode",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "S5MatchingCamera2",
                table: "Anode",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "S5SignStatus2",
                table: "Anode",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "S5TSFirstShooting",
                table: "Anode",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SS3S4MatchingCamera1",
                table: "Anode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SSignStatus1",
                table: "Anode",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Anode",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPlug",
                table: "ToSign");

            migrationBuilder.DropColumn(
                name: "NbActiveAlarms",
                table: "ToSign");

            migrationBuilder.DropColumn(
                name: "HasPlug",
                table: "ToMatch");

            migrationBuilder.DropColumn(
                name: "NbActiveAlarms",
                table: "ToMatch");

            migrationBuilder.DropColumn(
                name: "HasPlug",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "NbActiveAlarms",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "HasPlug",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "NbActiveAlarms",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "S1S2SignStatus1",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S1S2SignStatus2",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S1S2TSFirstShooting",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S3S4MatchingCamera2",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S3S4SignStatus1",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S3S4SignStatus2",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S3S4TSFirstShooting",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S5MatchingCamera1",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S5MatchingCamera2",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S5SignStatus2",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S5TSFirstShooting",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "SS3S4MatchingCamera1",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "SSignStatus1",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Anode");
        }
    }
}
