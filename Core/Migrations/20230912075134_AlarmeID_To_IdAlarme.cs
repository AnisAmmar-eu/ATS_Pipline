using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AlarmeID_To_IdAlarme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journal_Alarme_C_AlarmeID",
                table: "Journal");

            migrationBuilder.RenameColumn(
                name: "AlarmeID",
                table: "Journal",
                newName: "IdAlarme");

            migrationBuilder.RenameIndex(
                name: "IX_Journal_AlarmeID",
                table: "Journal",
                newName: "IX_Journal_IdAlarme");

            migrationBuilder.RenameColumn(
                name: "AlarmeID",
                table: "AlarmePLC",
                newName: "IdAlarme");

            migrationBuilder.AddForeignKey(
                name: "FK_Journal_Alarme_C_IdAlarme",
                table: "Journal",
                column: "IdAlarme",
                principalTable: "Alarme_C",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journal_Alarme_C_IdAlarme",
                table: "Journal");

            migrationBuilder.RenameColumn(
                name: "IdAlarme",
                table: "Journal",
                newName: "AlarmeID");

            migrationBuilder.RenameIndex(
                name: "IX_Journal_IdAlarme",
                table: "Journal",
                newName: "IX_Journal_AlarmeID");

            migrationBuilder.RenameColumn(
                name: "IdAlarme",
                table: "AlarmePLC",
                newName: "AlarmeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Journal_Alarme_C_AlarmeID",
                table: "Journal",
                column: "AlarmeID",
                principalTable: "Alarme_C",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
