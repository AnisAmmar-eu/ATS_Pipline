using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class RemovedCameraParams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CameraParam");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CameraParam",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcquisitionFrameRate = table.Column<double>(type: "float", nullable: false),
                    AcquisitionFrameRateEnable = table.Column<bool>(type: "bit", nullable: false),
                    AdaptiveNoiseSuppressionFactor = table.Column<double>(type: "float", nullable: false),
                    BalanceRatio = table.Column<double>(type: "float", nullable: false),
                    BlackLevel = table.Column<double>(type: "float", nullable: false),
                    ConvolutionMode = table.Column<bool>(type: "bit", nullable: false),
                    ExposureTime = table.Column<double>(type: "float", nullable: false),
                    Gain = table.Column<double>(type: "float", nullable: false),
                    Gamma = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<long>(type: "bigint", nullable: false),
                    PixelFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sharpness = table.Column<long>(type: "bigint", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TriggerActivation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerMode = table.Column<bool>(type: "bit", nullable: false),
                    TriggerSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraParam", x => x.ID);
                });
        }
    }
}
