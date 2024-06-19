using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class PlugNbAlarmToDo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPlug",
                table: "ToUnload",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NbActiveAlarms",
                table: "ToUnload",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasPlug",
                table: "ToLoad",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NbActiveAlarms",
                table: "ToLoad",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasPlug",
                table: "Dataset",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NbActiveAlarms",
                table: "Dataset",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPlug",
                table: "ToUnload");

            migrationBuilder.DropColumn(
                name: "NbActiveAlarms",
                table: "ToUnload");

            migrationBuilder.DropColumn(
                name: "HasPlug",
                table: "ToLoad");

            migrationBuilder.DropColumn(
                name: "NbActiveAlarms",
                table: "ToLoad");

            migrationBuilder.DropColumn(
                name: "HasPlug",
                table: "Dataset");

            migrationBuilder.DropColumn(
                name: "NbActiveAlarms",
                table: "Dataset");
        }
    }
}
