using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class addIOTDeviceStation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InFurnaceStatus",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "OutFurnaceStatus",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "IsDoubleAnode",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "S5Shooting_IsDoubleAnode",
                table: "Packet");

            migrationBuilder.RenameColumn(
                name: "TSClosed",
                table: "StationCycle",
                newName: "TSFirstShooting");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "oldestShooting",
                table: "IOTDevice",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "oldestShooting",
                table: "IOTDevice");

            migrationBuilder.RenameColumn(
                name: "TSFirstShooting",
                table: "StationCycle",
                newName: "TSClosed");

            migrationBuilder.AddColumn<int>(
                name: "InFurnaceStatus",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutFurnaceStatus",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDoubleAnode",
                table: "Packet",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "S5Shooting_IsDoubleAnode",
                table: "Packet",
                type: "bit",
                nullable: true);
        }
    }
}
