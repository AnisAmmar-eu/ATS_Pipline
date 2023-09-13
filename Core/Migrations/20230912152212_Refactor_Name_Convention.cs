using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Refactor_Name_Convention : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journal_Alarme_C_IdAlarme",
                table: "Journal");

            migrationBuilder.DropTable(
                name: "AlarmePLC");

            migrationBuilder.DropTable(
                name: "AlarmeTR");

            migrationBuilder.DropTable(
                name: "Alarme_C");

            migrationBuilder.RenameColumn(
                name: "TSLu",
                table: "Journal",
                newName: "TSRead");

            migrationBuilder.RenameColumn(
                name: "Lu",
                table: "Journal",
                newName: "Read");

            migrationBuilder.RenameColumn(
                name: "IdAlarme",
                table: "Journal",
                newName: "IDAlarm");

            migrationBuilder.RenameIndex(
                name: "IX_Journal_IdAlarme",
                table: "Journal",
                newName: "IX_Journal_IDAlarm");

            migrationBuilder.CreateTable(
                name: "AlarmC",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmC", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AlarmPLC",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDAlarm = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmPLC", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AlarmRT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDAlarm = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Station = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberNonRead = table.Column<int>(type: "int", nullable: true),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmRT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AlarmRT_AlarmC_IDAlarm",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Journal_AlarmC_IDAlarm",
                table: "Journal",
                column: "IDAlarm",
                principalTable: "AlarmC",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journal_AlarmC_IDAlarm",
                table: "Journal");

            migrationBuilder.DropTable(
                name: "AlarmPLC");

            migrationBuilder.DropTable(
                name: "AlarmRT");

            migrationBuilder.DropTable(
                name: "AlarmC");

            migrationBuilder.RenameColumn(
                name: "TSRead",
                table: "Journal",
                newName: "TSLu");

            migrationBuilder.RenameColumn(
                name: "Read",
                table: "Journal",
                newName: "Lu");

            migrationBuilder.RenameColumn(
                name: "IDAlarm",
                table: "Journal",
                newName: "IdAlarme");

            migrationBuilder.RenameIndex(
                name: "IX_Journal_IDAlarm",
                table: "Journal",
                newName: "IX_Journal_IdAlarme");

            migrationBuilder.CreateTable(
                name: "Alarme_C",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarme_C", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AlarmePLC",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlarme = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmePLC", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AlarmeTR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlarme = table.Column<int>(type: "int", nullable: false),
                    NombreNonLu = table.Column<int>(type: "int", nullable: true),
                    Station = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmeTR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlarmeTR_Alarme_C_IdAlarme",
                        column: x => x.IdAlarme,
                        principalTable: "Alarme_C",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlarmeTR_IdAlarme",
                table: "AlarmeTR",
                column: "IdAlarme",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Journal_Alarme_C_IdAlarme",
                table: "Journal",
                column: "IdAlarme",
                principalTable: "Alarme_C",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
