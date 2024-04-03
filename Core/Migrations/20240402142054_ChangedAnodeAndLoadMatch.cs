using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAnodeAndLoadMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToLoad_StationCycle_LoadableCycleID",
                table: "ToLoad");

            migrationBuilder.DropForeignKey(
                name: "FK_ToMatch_StationCycle_MatchableCycleID",
                table: "ToMatch");

            migrationBuilder.DropIndex(
                name: "IX_ToMatch_ShootingTS",
                table: "ToMatch");

            migrationBuilder.DropIndex(
                name: "IX_ToLoad_ShootingTS",
                table: "ToLoad");

            migrationBuilder.RenameColumn(
                name: "MatchableCycleID",
                table: "ToMatch",
                newName: "StationCycleID");

            migrationBuilder.RenameIndex(
                name: "IX_ToMatch_MatchableCycleID",
                table: "ToMatch",
                newName: "IX_ToMatch_StationCycleID");

            migrationBuilder.RenameColumn(
                name: "LoadableCycleID",
                table: "ToLoad",
                newName: "StationCycleID");

            migrationBuilder.RenameIndex(
                name: "IX_ToLoad_LoadableCycleID",
                table: "ToLoad",
                newName: "IX_ToLoad_StationCycleID");

            migrationBuilder.AddColumn<int>(
                name: "StationCycleID",
                table: "ToUnload",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StationCycleID",
                table: "ToSign",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "StationCycle",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.AddColumn<int>(
                name: "ChainMatchingCamera1",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChainMatchingCamera2",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StationCycleID",
                table: "Dataset",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ToUnload_StationCycleID",
                table: "ToUnload",
                column: "StationCycleID");

            migrationBuilder.CreateIndex(
                name: "IX_ToSign_StationCycleID",
                table: "ToSign",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IX_Dataset_StationCycleID",
                table: "Dataset");

            migrationBuilder.DropColumn(
                name: "StationCycleID",
                table: "ToUnload");

            migrationBuilder.DropColumn(
                name: "StationCycleID",
                table: "ToSign");

            migrationBuilder.DropColumn(
                name: "ChainMatchingCamera1",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "ChainMatchingCamera2",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "StationCycleID",
                table: "Dataset");

            migrationBuilder.RenameColumn(
                name: "StationCycleID",
                table: "ToMatch",
                newName: "MatchableCycleID");

            migrationBuilder.RenameIndex(
                name: "IX_ToMatch_StationCycleID",
                table: "ToMatch",
                newName: "IX_ToMatch_MatchableCycleID");

            migrationBuilder.RenameColumn(
                name: "StationCycleID",
                table: "ToLoad",
                newName: "LoadableCycleID");

            migrationBuilder.RenameIndex(
                name: "IX_ToLoad_StationCycleID",
                table: "ToLoad",
                newName: "IX_ToLoad_LoadableCycleID");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "StationCycle",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.CreateIndex(
                name: "IX_ToMatch_ShootingTS",
                table: "ToMatch",
                column: "ShootingTS");

            migrationBuilder.CreateIndex(
                name: "IX_ToLoad_ShootingTS",
                table: "ToLoad",
                column: "ShootingTS");

            migrationBuilder.AddForeignKey(
                name: "FK_ToLoad_StationCycle_LoadableCycleID",
                table: "ToLoad",
                column: "LoadableCycleID",
                principalTable: "StationCycle",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToMatch_StationCycle_MatchableCycleID",
                table: "ToMatch",
                column: "MatchableCycleID",
                principalTable: "StationCycle",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
