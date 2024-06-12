using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class ReAddedChainCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChainCycleID",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_ChainCycleID",
                table: "StationCycle",
                column: "ChainCycleID",
                unique: true,
                filter: "[ChainCycleID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_StationCycle_StationCycle_ChainCycleID",
                table: "StationCycle",
                column: "ChainCycleID",
                principalTable: "StationCycle",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationCycle_StationCycle_ChainCycleID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_ChainCycleID",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "ChainCycleID",
                table: "StationCycle");
        }
    }
}
