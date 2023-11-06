using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Anode_NullableCycleIDs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Anode_S3S4CycleID",
                table: "Anode");

            migrationBuilder.AlterColumn<int>(
                name: "S3S4CycleID",
                table: "Anode",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Anode_S3S4CycleID",
                table: "Anode",
                column: "S3S4CycleID",
                unique: true,
                filter: "[S3S4CycleID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Anode_S3S4CycleID",
                table: "Anode");

            migrationBuilder.AlterColumn<int>(
                name: "S3S4CycleID",
                table: "Anode",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Anode_S3S4CycleID",
                table: "Anode",
                column: "S3S4CycleID",
                unique: true);
        }
    }
}
