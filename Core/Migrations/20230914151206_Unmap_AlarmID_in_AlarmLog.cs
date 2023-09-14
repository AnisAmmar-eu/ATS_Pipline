using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Unmap_AlarmID_in_AlarmLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlarmLog_AlarmC_AlarmID",
                table: "AlarmLog");

            migrationBuilder.DropIndex(
                name: "IX_AlarmLog_AlarmID",
                table: "AlarmLog");

            migrationBuilder.DropColumn(
                name: "AlarmID",
                table: "AlarmLog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlarmID",
                table: "AlarmLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AlarmLog_AlarmID",
                table: "AlarmLog",
                column: "AlarmID");

            migrationBuilder.AddForeignKey(
                name: "FK_AlarmLog_AlarmC_AlarmID",
                table: "AlarmLog",
                column: "AlarmID",
                principalTable: "AlarmC",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
