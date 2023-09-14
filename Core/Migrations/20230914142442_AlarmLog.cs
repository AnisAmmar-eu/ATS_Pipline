using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AlarmLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlarmRT_AlarmC_IDAlarm",
                table: "AlarmRT");

            migrationBuilder.DropTable(
                name: "Journal");

            migrationBuilder.DropIndex(
                name: "IX_AlarmRT_IDAlarm",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AlarmPLC");

            migrationBuilder.AddColumn<int>(
                name: "AlarmCID",
                table: "AlarmRT",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AlarmPLC",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AlarmLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HasChanged = table.Column<bool>(type: "bit", nullable: true),
                    IRID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IDAlarm = table.Column<int>(type: "int", nullable: false),
                    Station = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAck = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TSRaised = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TSClear = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    TSRead = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TSGet = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AlarmID = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AlarmLog_AlarmC_AlarmID",
                        column: x => x.AlarmID,
                        principalTable: "AlarmC",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlarmRT_AlarmCID",
                table: "AlarmRT",
                column: "AlarmCID");

            migrationBuilder.CreateIndex(
                name: "IX_AlarmLog_AlarmID",
                table: "AlarmLog",
                column: "AlarmID");

            migrationBuilder.AddForeignKey(
                name: "FK_AlarmRT_AlarmC_AlarmCID",
                table: "AlarmRT",
                column: "AlarmCID",
                principalTable: "AlarmC",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlarmRT_AlarmC_AlarmCID",
                table: "AlarmRT");

            migrationBuilder.DropTable(
                name: "AlarmLog");

            migrationBuilder.DropIndex(
                name: "IX_AlarmRT_AlarmCID",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "AlarmCID",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AlarmPLC");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AlarmPLC",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Journal",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDAlarm = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<int>(type: "int", nullable: true),
                    Station = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status0 = table.Column<int>(type: "int", nullable: true),
                    Status1 = table.Column<int>(type: "int", nullable: true),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TS0 = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TS1 = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TSRead = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journal", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Journal_AlarmC_IDAlarm",
                        column: x => x.IDAlarm,
                        principalTable: "AlarmC",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlarmRT_IDAlarm",
                table: "AlarmRT",
                column: "IDAlarm",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Journal_IDAlarm",
                table: "Journal",
                column: "IDAlarm");

            migrationBuilder.AddForeignKey(
                name: "FK_AlarmRT_AlarmC_IDAlarm",
                table: "AlarmRT",
                column: "IDAlarm",
                principalTable: "AlarmC",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
