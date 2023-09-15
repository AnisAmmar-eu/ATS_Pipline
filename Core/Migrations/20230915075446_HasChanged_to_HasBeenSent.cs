using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class HasChanged_to_HasBeenSent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasChanged",
                table: "AlarmLog");

            migrationBuilder.AddColumn<bool>(
                name: "HasBeenSent",
                table: "AlarmLog",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenSent",
                table: "AlarmLog");

            migrationBuilder.AddColumn<bool>(
                name: "HasChanged",
                table: "AlarmLog",
                type: "bit",
                nullable: true);
        }
    }
}
