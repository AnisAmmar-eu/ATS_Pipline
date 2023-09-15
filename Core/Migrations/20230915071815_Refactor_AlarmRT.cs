using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Refactor_AlarmRT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberNonRead",
                table: "AlarmRT",
                newName: "NbNonAck");

            migrationBuilder.AddColumn<string>(
                name: "IRID",
                table: "AlarmRT",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TSClear",
                table: "AlarmRT",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TSRaised",
                table: "AlarmRT",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "Station",
                table: "AlarmLog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IRID",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "TSClear",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "TSRaised",
                table: "AlarmRT");

            migrationBuilder.RenameColumn(
                name: "NbNonAck",
                table: "AlarmRT",
                newName: "NumberNonRead");

            migrationBuilder.AlterColumn<string>(
                name: "Station",
                table: "AlarmLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
