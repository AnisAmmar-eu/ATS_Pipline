using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class RepositoryStruct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journal_Alarme_C_IdAlarme",
                table: "Journal");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Journal",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "IdAlarme",
                table: "Journal",
                newName: "AlarmeID");

            migrationBuilder.RenameIndex(
                name: "IX_Journal_IdAlarme",
                table: "Journal",
                newName: "IX_Journal_AlarmeID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AlarmePLC",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "IdAlarme",
                table: "AlarmePLC",
                newName: "AlarmeID");

            migrationBuilder.RenameColumn(
                name: "IdAlarm",
                table: "Alarme_C",
                newName: "ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "TS",
                table: "Journal",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TS",
                table: "Alarme_C",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddForeignKey(
                name: "FK_Journal_Alarme_C_AlarmeID",
                table: "Journal",
                column: "AlarmeID",
                principalTable: "Alarme_C",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journal_Alarme_C_AlarmeID",
                table: "Journal");

            migrationBuilder.DropColumn(
                name: "TS",
                table: "Alarme_C");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Journal",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AlarmeID",
                table: "Journal",
                newName: "IdAlarme");

            migrationBuilder.RenameIndex(
                name: "IX_Journal_AlarmeID",
                table: "Journal",
                newName: "IX_Journal_IdAlarme");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "AlarmePLC",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AlarmeID",
                table: "AlarmePLC",
                newName: "IdAlarme");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Alarme_C",
                newName: "IdAlarm");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "TS",
                table: "Journal",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddForeignKey(
                name: "FK_Journal_Alarme_C_IdAlarme",
                table: "Journal",
                column: "IdAlarme",
                principalTable: "Alarme_C",
                principalColumn: "IdAlarm",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
