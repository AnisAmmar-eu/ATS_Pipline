using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangedToDo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CycleID",
                table: "ToUnload");

            migrationBuilder.DropColumn(
                name: "CycleID",
                table: "ToSign");

            migrationBuilder.DropColumn(
                name: "CycleID",
                table: "ToMatch");

            migrationBuilder.DropColumn(
                name: "CycleID",
                table: "ToLoad");

            migrationBuilder.DropColumn(
                name: "CycleID",
                table: "Dataset");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CycleID",
                table: "ToUnload",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CycleID",
                table: "ToSign",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CycleID",
                table: "ToMatch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CycleID",
                table: "ToLoad",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CycleID",
                table: "Dataset",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
