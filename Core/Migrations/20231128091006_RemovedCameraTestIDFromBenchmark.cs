using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCameraTestIDFromBenchmark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BenchmarkTest_CameraTest_CameraTestID",
                table: "BenchmarkTest");

            migrationBuilder.DropIndex(
                name: "IX_BenchmarkTest_CameraTestID",
                table: "BenchmarkTest");

            migrationBuilder.DropColumn(
                name: "CameraTestID",
                table: "BenchmarkTest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CameraTestID",
                table: "BenchmarkTest",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkTest_CameraTestID",
                table: "BenchmarkTest",
                column: "CameraTestID");

            migrationBuilder.AddForeignKey(
                name: "FK_BenchmarkTest_CameraTest_CameraTestID",
                table: "BenchmarkTest",
                column: "CameraTestID",
                principalTable: "CameraTest",
                principalColumn: "ID");
        }
    }
}
