using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alarme_C",
                columns: table => new
                {
                    IdAlarm = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarme_C", x => x.IdAlarm);
                });

            migrationBuilder.CreateTable(
                name: "AlarmePLC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlarme = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmePLC", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlarmeTR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlarme = table.Column<int>(type: "int", nullable: false),
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
                        principalColumn: "IdAlarm",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Journal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlarme = table.Column<int>(type: "int", nullable: false),
                    Status1 = table.Column<int>(type: "int", nullable: true),
                    TS1 = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status0 = table.Column<int>(type: "int", nullable: true),
                    TS0 = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Journal_Alarme_C_IdAlarme",
                        column: x => x.IdAlarme,
                        principalTable: "Alarme_C",
                        principalColumn: "IdAlarm",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlarmeTR_IdAlarme",
                table: "AlarmeTR",
                column: "IdAlarme",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Journal_IdAlarme",
                table: "Journal",
                column: "IdAlarme");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlarmePLC");

            migrationBuilder.DropTable(
                name: "AlarmeTR");

            migrationBuilder.DropTable(
                name: "Journal");

            migrationBuilder.DropTable(
                name: "Alarme_C");
        }
    }
}
