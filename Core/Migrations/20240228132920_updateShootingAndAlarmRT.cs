using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class updateShootingAndAlarmRT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnodeSize",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Cam01Temp",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Cam02Temp",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SyncIndex",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "TT01",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "NbNonAck",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "TSClear",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "NbNonAck",
                table: "AlarmCycle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnodeSize",
                table: "Packet",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "SyncIndex",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TT01",
                table: "Packet",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NbNonAck",
                table: "AlarmRT",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TSClear",
                table: "AlarmRT",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NbNonAck",
                table: "AlarmCycle",
                type: "int",
                nullable: true);
        }
    }
}
