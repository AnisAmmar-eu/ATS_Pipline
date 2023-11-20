using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class CameraTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CameraTest",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraTest", x => x.ID);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BenchmarkTest_CameraTest_CameraID",
                table: "BenchmarkTest");

            migrationBuilder.DropTable(
                name: "CameraTest");

            migrationBuilder.DropIndex(
                name: "IX_BenchmarkTest_CameraID",
                table: "BenchmarkTest");
        }
    }
}
