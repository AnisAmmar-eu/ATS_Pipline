using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class CleanedAnode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadableQueue");

            migrationBuilder.DropTable(
                name: "MatchableStack");

            migrationBuilder.DropColumn(
                name: "ClosedTS",
                table: "Anode");

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

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Anode");

            migrationBuilder.CreateTable(
                name: "Dataset",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SANfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CycleID = table.Column<int>(type: "int", nullable: false),
                    CycleRID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraID = table.Column<int>(type: "int", nullable: false),
                    StationID = table.Column<int>(type: "int", nullable: false),
                    AnodeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShootingTS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dataset", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ToLoad",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadableCycleID = table.Column<int>(type: "int", nullable: false),
                    DataSetID = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CycleID = table.Column<int>(type: "int", nullable: false),
                    CycleRID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraID = table.Column<int>(type: "int", nullable: false),
                    StationID = table.Column<int>(type: "int", nullable: false),
                    AnodeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShootingTS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToLoad", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ToLoad_StationCycle_LoadableCycleID",
                        column: x => x.LoadableCycleID,
                        principalTable: "StationCycle",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToMatch",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchableCycleID = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CycleID = table.Column<int>(type: "int", nullable: false),
                    CycleRID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraID = table.Column<int>(type: "int", nullable: false),
                    StationID = table.Column<int>(type: "int", nullable: false),
                    AnodeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShootingTS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToMatch", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ToMatch_StationCycle_MatchableCycleID",
                        column: x => x.MatchableCycleID,
                        principalTable: "StationCycle",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToNotify",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SynchronisationKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToNotify", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ToSign",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CycleID = table.Column<int>(type: "int", nullable: false),
                    CycleRID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraID = table.Column<int>(type: "int", nullable: false),
                    StationID = table.Column<int>(type: "int", nullable: false),
                    AnodeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShootingTS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToSign", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ToUnload",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SANfile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CycleID = table.Column<int>(type: "int", nullable: false),
                    CycleRID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraID = table.Column<int>(type: "int", nullable: false),
                    StationID = table.Column<int>(type: "int", nullable: false),
                    AnodeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShootingTS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToUnload", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToLoad_LoadableCycleID",
                table: "ToLoad",
                column: "LoadableCycleID");

            migrationBuilder.CreateIndex(
                name: "IX_ToLoad_ShootingTS",
                table: "ToLoad",
                column: "ShootingTS");

            migrationBuilder.CreateIndex(
                name: "IX_ToMatch_MatchableCycleID",
                table: "ToMatch",
                column: "MatchableCycleID");

            migrationBuilder.CreateIndex(
                name: "IX_ToMatch_ShootingTS",
                table: "ToMatch",
                column: "ShootingTS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dataset");

            migrationBuilder.DropTable(
                name: "ToLoad");

            migrationBuilder.DropTable(
                name: "ToMatch");

            migrationBuilder.DropTable(
                name: "ToNotify");

            migrationBuilder.DropTable(
                name: "ToSign");

            migrationBuilder.DropTable(
                name: "ToUnload");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ClosedTS",
                table: "Anode",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "S1S2CycleStationID",
                table: "Anode",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "S1S2CycleTS",
                table: "Anode",
                type: "datetimeoffset",
                nullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Anode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LoadableQueue",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadableCycleID = table.Column<int>(type: "int", nullable: false),
                    CycleTS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DataSetID = table.Column<int>(type: "int", nullable: false),
                    SAN1Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAN2Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadableQueue", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LoadableQueue_StationCycle_LoadableCycleID",
                        column: x => x.LoadableCycleID,
                        principalTable: "StationCycle",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchableStack",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchableCycleID = table.Column<int>(type: "int", nullable: false),
                    CycleTS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DataSetID = table.Column<int>(type: "int", nullable: false),
                    SAN1Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAN2Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchableStack", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MatchableStack_StationCycle_MatchableCycleID",
                        column: x => x.MatchableCycleID,
                        principalTable: "StationCycle",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadableQueue_CycleTS",
                table: "LoadableQueue",
                column: "CycleTS");

            migrationBuilder.CreateIndex(
                name: "IX_LoadableQueue_LoadableCycleID",
                table: "LoadableQueue",
                column: "LoadableCycleID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchableStack_CycleTS",
                table: "MatchableStack",
                column: "CycleTS");

            migrationBuilder.CreateIndex(
                name: "IX_MatchableStack_MatchableCycleID",
                table: "MatchableStack",
                column: "MatchableCycleID");
        }
    }
}
