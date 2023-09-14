using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AlarmRT_Refactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlarmRT_AlarmC_AlarmCID",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AlarmRT");

            migrationBuilder.RenameColumn(
                name: "AlarmCID",
                table: "AlarmRT",
                newName: "AlarmID");

            migrationBuilder.RenameIndex(
                name: "IX_AlarmRT_AlarmCID",
                table: "AlarmRT",
                newName: "IX_AlarmRT_AlarmID");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AlarmRT",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                name: "FK_AlarmRT_AlarmC_AlarmID",
                table: "AlarmRT");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AlarmRT");

            migrationBuilder.RenameColumn(
                name: "AlarmID",
                table: "AlarmRT",
                newName: "AlarmCID");

            migrationBuilder.RenameIndex(
                name: "IX_AlarmRT_AlarmID",
                table: "AlarmRT",
                newName: "IX_AlarmRT_AlarmCID");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AlarmRT",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AlarmRT_AlarmC_AlarmCID",
                table: "AlarmRT",
                column: "AlarmCID",
                principalTable: "AlarmC",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
