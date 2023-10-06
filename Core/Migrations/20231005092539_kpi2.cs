using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class kpi2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Period",
                table: "KPIRT",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StationID",
                table: "KPIRT",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "KPIRT",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Period",
                table: "KPILog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StationID",
                table: "KPILog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "KPILog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "KPIC",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "KPIC",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RID",
                table: "KPIC",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Period",
                table: "KPIRT");

            migrationBuilder.DropColumn(
                name: "StationID",
                table: "KPIRT");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "KPIRT");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "KPILog");

            migrationBuilder.DropColumn(
                name: "StationID",
                table: "KPILog");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "KPILog");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "KPIC");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "KPIC");

            migrationBuilder.DropColumn(
                name: "RID",
                table: "KPIC");
        }
    }
}
