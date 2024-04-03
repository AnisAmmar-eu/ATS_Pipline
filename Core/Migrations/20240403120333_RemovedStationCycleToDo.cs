using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class RemovedStationCycleToDo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dataset_StationCycle_StationCycleID",
                table: "Dataset");

            migrationBuilder.DropForeignKey(
                name: "FK_ToLoad_StationCycle_StationCycleID",
                table: "ToLoad");

            migrationBuilder.DropForeignKey(
                name: "FK_ToMatch_StationCycle_StationCycleID",
                table: "ToMatch");

            migrationBuilder.DropForeignKey(
                name: "FK_ToSign_StationCycle_StationCycleID",
                table: "ToSign");

            migrationBuilder.DropForeignKey(
                name: "FK_ToUnload_StationCycle_StationCycleID",
                table: "ToUnload");

            migrationBuilder.DropIndex(
                name: "IX_ToUnload_StationCycleID",
                table: "ToUnload");

            migrationBuilder.DropIndex(
                name: "IX_ToSign_StationCycleID",
                table: "ToSign");

            migrationBuilder.DropIndex(
                name: "IX_ToMatch_StationCycleID",
                table: "ToMatch");

            migrationBuilder.DropIndex(
                name: "IX_ToLoad_StationCycleID",
                table: "ToLoad");

            migrationBuilder.DropIndex(
                name: "IX_Dataset_StationCycleID",
                table: "Dataset");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ToUnload_StationCycleID",
                table: "ToUnload",
                column: "StationCycleID");

            migrationBuilder.CreateIndex(
                name: "IX_ToSign_StationCycleID",
                table: "ToSign",
                column: "StationCycleID");

            migrationBuilder.CreateIndex(
                name: "IX_ToMatch_StationCycleID",
                table: "ToMatch",
                column: "StationCycleID");

            migrationBuilder.CreateIndex(
                name: "IX_ToLoad_StationCycleID",
                table: "ToLoad",
                column: "StationCycleID");

            migrationBuilder.CreateIndex(
                name: "IX_Dataset_StationCycleID",
                table: "Dataset",
                column: "StationCycleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Dataset_StationCycle_StationCycleID",
                table: "Dataset",
                column: "StationCycleID",
                principalTable: "StationCycle",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToLoad_StationCycle_StationCycleID",
                table: "ToLoad",
                column: "StationCycleID",
                principalTable: "StationCycle",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToMatch_StationCycle_StationCycleID",
                table: "ToMatch",
                column: "StationCycleID",
                principalTable: "StationCycle",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToSign_StationCycle_StationCycleID",
                table: "ToSign",
                column: "StationCycleID",
                principalTable: "StationCycle",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToUnload_StationCycle_StationCycleID",
                table: "ToUnload",
                column: "StationCycleID",
                principalTable: "StationCycle",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
