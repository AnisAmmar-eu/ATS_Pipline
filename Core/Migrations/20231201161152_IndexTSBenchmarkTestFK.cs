using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class IndexTSBenchmarkTestFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TSIndex",
                table: "BenchmarkTest",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkTest_CameraID",
                table: "BenchmarkTest",
                column: "CameraID");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkTest_TSIndex",
                table: "BenchmarkTest",
                column: "TSIndex");

            migrationBuilder.AddForeignKey(
                name: "FK_BenchmarkTest_CameraTest_CameraID",
                table: "BenchmarkTest",
                column: "CameraID",
                principalTable: "CameraTest",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BenchmarkTest_CameraTest_CameraID",
                table: "BenchmarkTest");

            migrationBuilder.DropIndex(
                name: "IX_BenchmarkTest_CameraID",
                table: "BenchmarkTest");

            migrationBuilder.DropIndex(
                name: "IX_BenchmarkTest_TSIndex",
                table: "BenchmarkTest");

            migrationBuilder.DropColumn(
                name: "TSIndex",
                table: "BenchmarkTest");
        }
    }
}
