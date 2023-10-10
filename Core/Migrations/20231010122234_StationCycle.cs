using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class StationCycle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CycleStationRID",
                table: "Packet",
                newName: "StationCycleRID");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "HasError",
                table: "Packet",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CameraParam",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TriggerMode = table.Column<bool>(type: "bit", nullable: false),
                    TriggerSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerActivation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExposureTime = table.Column<double>(type: "float", nullable: false),
                    PixelFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width = table.Column<long>(type: "bigint", nullable: false),
                    Height = table.Column<long>(type: "bigint", nullable: false),
                    AcquisitionFrameRateEnable = table.Column<bool>(type: "bit", nullable: false),
                    Gain = table.Column<double>(type: "float", nullable: false),
                    BlackLevel = table.Column<double>(type: "float", nullable: false),
                    Gamma = table.Column<double>(type: "float", nullable: false),
                    BalanceRatio = table.Column<double>(type: "float", nullable: false),
                    ConvolutionMode = table.Column<bool>(type: "bit", nullable: false),
                    AdaptiveNoiseSuppressionFactor = table.Column<double>(type: "float", nullable: false),
                    Sharpness = table.Column<long>(type: "bigint", nullable: false),
                    AcquisitionFrameRate = table.Column<double>(type: "float", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraParam", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StationCycle",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnodeType = table.Column<int>(type: "int", nullable: false),
                    AnnounceID = table.Column<int>(type: "int", nullable: false),
                    RID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TSClosed = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AnnouncementStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnnouncementID = table.Column<int>(type: "int", nullable: false),
                    DetectionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetectionID = table.Column<int>(type: "int", nullable: false),
                    ShootingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShootingID = table.Column<int>(type: "int", nullable: false),
                    AlarmListStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlarmListID = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationCycle", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StationCycle_Packet_AlarmListID",
                        column: x => x.AlarmListID,
                        principalTable: "Packet",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_StationCycle_Packet_AnnouncementID",
                        column: x => x.AnnouncementID,
                        principalTable: "Packet",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_StationCycle_Packet_DetectionID",
                        column: x => x.DetectionID,
                        principalTable: "Packet",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_StationCycle_Packet_ShootingID",
                        column: x => x.ShootingID,
                        principalTable: "Packet",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_AlarmListID",
                table: "StationCycle",
                column: "AlarmListID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_AnnouncementID",
                table: "StationCycle",
                column: "AnnouncementID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_DetectionID",
                table: "StationCycle",
                column: "DetectionID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_ShootingID",
                table: "StationCycle",
                column: "ShootingID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CameraParam");

            migrationBuilder.DropTable(
                name: "StationCycle");

            migrationBuilder.DropColumn(
                name: "HasError",
                table: "Packet");

            migrationBuilder.RenameColumn(
                name: "StationCycleRID",
                table: "Packet",
                newName: "CycleStationRID");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Packet",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
