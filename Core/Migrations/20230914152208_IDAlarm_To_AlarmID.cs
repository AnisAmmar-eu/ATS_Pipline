using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class IDAlarm_To_AlarmID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlarmLog_AlarmC_IDAlarm",
                table: "AlarmLog");

            migrationBuilder.DropForeignKey(
                name: "FK_AlarmRT_AlarmC_IDAlarm",
                table: "AlarmRT");

            migrationBuilder.RenameColumn(
                name: "IDAlarm",
                table: "AlarmRT",
                newName: "AlarmID");

            migrationBuilder.RenameIndex(
                name: "IX_AlarmRT_IDAlarm",
                table: "AlarmRT",
                newName: "IX_AlarmRT_AlarmID");

            migrationBuilder.RenameColumn(
                name: "IDAlarm",
                table: "AlarmPLC",
                newName: "AlarmID");

            migrationBuilder.RenameColumn(
                name: "IDAlarm",
                table: "AlarmLog",
                newName: "AlarmID");

            migrationBuilder.RenameIndex(
                name: "IX_AlarmLog_IDAlarm",
                table: "AlarmLog",
                newName: "IX_AlarmLog_AlarmID");

            migrationBuilder.AddForeignKey(
                name: "FK_AlarmLog_AlarmC_AlarmID",
                table: "AlarmLog",
                column: "AlarmID",
                principalTable: "AlarmC",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlarmRT_AlarmC_AlarmID",
                table: "AlarmRT",
                column: "AlarmID",
                principalTable: "AlarmC",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlarmLog_AlarmC_AlarmID",
                table: "AlarmLog");

            migrationBuilder.DropForeignKey(
                name: "FK_AlarmRT_AlarmC_AlarmID",
                table: "AlarmRT");

            migrationBuilder.RenameColumn(
                name: "AlarmID",
                table: "AlarmRT",
                newName: "IDAlarm");

            migrationBuilder.RenameIndex(
                name: "IX_AlarmRT_AlarmID",
                table: "AlarmRT",
                newName: "IX_AlarmRT_IDAlarm");

            migrationBuilder.RenameColumn(
                name: "AlarmID",
                table: "AlarmPLC",
                newName: "IDAlarm");

            migrationBuilder.RenameColumn(
                name: "AlarmID",
                table: "AlarmLog",
                newName: "IDAlarm");

            migrationBuilder.RenameIndex(
                name: "IX_AlarmLog_AlarmID",
                table: "AlarmLog",
                newName: "IX_AlarmLog_IDAlarm");

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
    }
}
