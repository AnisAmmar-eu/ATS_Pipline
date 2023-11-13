using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class index_on_anode_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BenchmarkTest_StationID",
                table: "BenchmarkTest");

            migrationBuilder.AlterColumn<string>(
                name: "AnodeType",
                table: "BenchmarkTest",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkTest_AnodeType",
                table: "BenchmarkTest",
                column: "AnodeType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BenchmarkTest_AnodeType",
                table: "BenchmarkTest");

            migrationBuilder.AlterColumn<string>(
                name: "AnodeType",
                table: "BenchmarkTest",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkTest_StationID",
                table: "BenchmarkTest",
                column: "StationID");
        }
    }
}
