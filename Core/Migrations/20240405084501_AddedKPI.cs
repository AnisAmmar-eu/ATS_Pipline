using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedKPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KPILog");

            migrationBuilder.DropTable(
                name: "KPIRT");

            migrationBuilder.DropTable(
                name: "KPIC");

            migrationBuilder.DropColumn(
                name: "CameraID",
                table: "ToUnload");

            migrationBuilder.DropColumn(
                name: "CameraID",
                table: "ToMatch");

            migrationBuilder.AddColumn<int>(
                name: "KPIID",
                table: "StationCycle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "MatchingTS",
                table: "StationCycle",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reinit",
                table: "IOTDevice",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KPI",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nbCandidats = table.Column<int>(type: "int", nullable: false),
                    threshold = table.Column<int>(type: "int", nullable: false),
                    MScore = table.Column<double>(type: "float", nullable: false),
                    NMminScore = table.Column<double>(type: "float", nullable: false),
                    NMmaxScore = table.Column<double>(type: "float", nullable: false),
                    NMAvg = table.Column<double>(type: "float", nullable: false),
                    NMStdev = table.Column<double>(type: "float", nullable: false),
                    computeTime = table.Column<double>(type: "float", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPI", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TenBestMatch",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rank = table.Column<int>(type: "int", nullable: false),
                    anodeID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    score = table.Column<int>(type: "int", nullable: false),
                    KPIID = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenBestMatch", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TenBestMatch_KPI_KPIID",
                        column: x => x.KPIID,
                        principalTable: "KPI",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_KPIID",
                table: "StationCycle",
                column: "KPIID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenBestMatch_KPIID",
                table: "TenBestMatch",
                column: "KPIID");

            migrationBuilder.AddForeignKey(
                name: "FK_StationCycle_KPI_KPIID",
                table: "StationCycle",
                column: "KPIID",
                principalTable: "KPI",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationCycle_KPI_KPIID",
                table: "StationCycle");

            migrationBuilder.DropTable(
                name: "TenBestMatch");

            migrationBuilder.DropTable(
                name: "KPI");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_KPIID",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "KPIID",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "MatchingTS",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "Reinit",
                table: "IOTDevice");

            migrationBuilder.AddColumn<int>(
                name: "CameraID",
                table: "ToUnload",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CameraID",
                table: "ToMatch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "KPIC",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RID = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    KPICID = table.Column<int>(type: "int", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    KPICID = table.Column<int>(type: "int", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
    }
}
