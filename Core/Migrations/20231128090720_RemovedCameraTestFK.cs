using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCameraTestFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BenchmarkTest_CameraTest_CameraID",
                table: "BenchmarkTest");

            migrationBuilder.DropIndex(
                name: "IX_BenchmarkTest_CameraID",
                table: "BenchmarkTest");

            migrationBuilder.AlterColumn<int>(
                name: "AnodeType",
                table: "BenchmarkTest",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.AddColumn<int>(
                name: "CameraTestID",
                table: "BenchmarkTest",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BenchmarkTest",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BenchmarkTest");

            migrationBuilder.AlterColumn<string>(
                name: "AnodeType",
                table: "BenchmarkTest",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkTest_CameraID",
                table: "BenchmarkTest",
                column: "CameraID");

            migrationBuilder.AddForeignKey(
                name: "FK_BenchmarkTest_CameraTest_CameraID",
                table: "BenchmarkTest",
                column: "CameraID",
                principalTable: "CameraTest",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
