using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    public partial class kpi : Migration
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KPIC",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIC", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KPILog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    KPICID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPILog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KPILog_KPIC_KPICID",
                        column: x => x.KPICID,
                        principalTable: "KPIC",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KPIRT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    KPICID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIRT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KPIRT_KPIC_KPICID",
                        column: x => x.KPICID,
                        principalTable: "KPIC",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KPILog_KPICID",
                table: "KPILog",
                column: "KPICID");

            migrationBuilder.CreateIndex(
                name: "IX_KPIRT_KPICID",
                table: "KPIRT",
                column: "KPICID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KPILog");

            migrationBuilder.DropTable(
                name: "KPIRT");

            migrationBuilder.DropTable(
                name: "KPIC");
        }
    }
}
