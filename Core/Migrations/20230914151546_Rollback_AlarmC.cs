using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Rollback_AlarmC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlarmRT_AlarmC_AlarmID",
                table: "AlarmRT");

            migrationBuilder.DropIndex(
                name: "IX_AlarmRT_AlarmID",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "AlarmID",
                table: "AlarmRT");

            migrationBuilder.CreateIndex(
                name: "IX_AlarmRT_IDAlarm",
                table: "AlarmRT",
                column: "IDAlarm",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlarmLog_IDAlarm",
                table: "AlarmLog",
                column: "IDAlarm");

            migrationBuilder.AddForeignKey(
                name: "FK_AlarmLog_AlarmC_IDAlarm",
                table: "AlarmLog",
                column: "IDAlarm",
                principalTable: "AlarmC",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlarmRT_AlarmC_IDAlarm",
                table: "AlarmRT",
                column: "IDAlarm",
                principalTable: "AlarmC",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlarmLog_AlarmC_IDAlarm",
                table: "AlarmLog");

            migrationBuilder.DropForeignKey(
                name: "FK_AlarmRT_AlarmC_IDAlarm",
                table: "AlarmRT");

            migrationBuilder.DropIndex(
                name: "IX_AlarmRT_IDAlarm",
                table: "AlarmRT");

            migrationBuilder.DropIndex(
                name: "IX_AlarmLog_IDAlarm",
                table: "AlarmLog");

            migrationBuilder.AddColumn<int>(
                name: "AlarmID",
                table: "AlarmRT",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AlarmRT_AlarmID",
                table: "AlarmRT",
                column: "AlarmID");

            migrationBuilder.AddForeignKey(
                name: "FK_AlarmRT_AlarmC_AlarmID",
                table: "AlarmRT",
                column: "AlarmID",
                principalTable: "AlarmC",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
