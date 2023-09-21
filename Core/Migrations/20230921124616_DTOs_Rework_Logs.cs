using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class DTOs_Rework_Logs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IRID",
                table: "AlarmLog");

            migrationBuilder.DropColumn(
                name: "IRID",
                table: "AlarmCycle");

            migrationBuilder.DropColumn(
                name: "Station",
                table: "AlarmCycle");

            migrationBuilder.DropColumn(
                name: "TSClear",
                table: "AlarmCycle");

            migrationBuilder.DropColumn(
                name: "TSRaised",
                table: "AlarmCycle");

            migrationBuilder.AddColumn<string>(
                name: "PacketType",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PacketType",
                table: "Packet");

            migrationBuilder.AddColumn<string>(
                name: "IRID",
                table: "AlarmLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IRID",
                table: "AlarmCycle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Station",
                table: "AlarmCycle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TSClear",
                table: "AlarmCycle",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TSRaised",
                table: "AlarmCycle",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
