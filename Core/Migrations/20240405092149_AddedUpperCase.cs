using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedUpperCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StationCycle_KPIID",
                table: "StationCycle");

            migrationBuilder.RenameColumn(
                name: "score",
                table: "TenBestMatch",
                newName: "Score");

            migrationBuilder.RenameColumn(
                name: "rank",
                table: "TenBestMatch",
                newName: "Rank");

            migrationBuilder.RenameColumn(
                name: "anodeID",
                table: "TenBestMatch",
                newName: "AnodeID");

            migrationBuilder.RenameColumn(
                name: "threshold",
                table: "KPI",
                newName: "Threshold");

            migrationBuilder.RenameColumn(
                name: "nbCandidats",
                table: "KPI",
                newName: "NbCandidats");

            migrationBuilder.RenameColumn(
                name: "computeTime",
                table: "KPI",
                newName: "ComputeTime");

            migrationBuilder.AlterColumn<int>(
                name: "KPIID",
                table: "StationCycle",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_KPIID",
                table: "StationCycle",
                column: "KPIID",
                unique: true,
                filter: "[KPIID] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StationCycle_KPIID",
                table: "StationCycle");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "TenBestMatch",
                newName: "score");

            migrationBuilder.RenameColumn(
                name: "Rank",
                table: "TenBestMatch",
                newName: "rank");

            migrationBuilder.RenameColumn(
                name: "AnodeID",
                table: "TenBestMatch",
                newName: "anodeID");

            migrationBuilder.RenameColumn(
                name: "Threshold",
                table: "KPI",
                newName: "threshold");

            migrationBuilder.RenameColumn(
                name: "NbCandidats",
                table: "KPI",
                newName: "nbCandidats");

            migrationBuilder.RenameColumn(
                name: "ComputeTime",
                table: "KPI",
                newName: "computeTime");

            migrationBuilder.AlterColumn<int>(
                name: "KPIID",
                table: "StationCycle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_KPIID",
                table: "StationCycle",
                column: "KPIID",
                unique: true);
        }
    }
}
