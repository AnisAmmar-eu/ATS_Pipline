using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class CycleDataInAnodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "S1S2CycleStationID",
                table: "Anode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "S1S2CycleTS",
                table: "Anode",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "S3S4CycleStationID",
                table: "Anode",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "S3S4CycleTS",
                table: "Anode",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "S5CycleTS",
                table: "Anode",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "S1S2CycleStationID",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S1S2CycleTS",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S3S4CycleStationID",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S3S4CycleTS",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "S5CycleTS",
                table: "Anode");
        }
    }
}
