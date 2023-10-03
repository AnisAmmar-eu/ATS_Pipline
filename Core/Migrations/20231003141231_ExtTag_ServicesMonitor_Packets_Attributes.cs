using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class ExtTag_ServicesMonitor_Packets_Attributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ServicesMonitor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IPAdress",
                table: "ServicesMonitor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsConnected",
                table: "ServicesMonitor",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ServicesMonitor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RID",
                table: "ServicesMonitor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AnodeSize",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMismatched",
                table: "Packet",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MeasuredType",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentValue",
                table: "ExtTag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ExtTag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasNewValue",
                table: "ExtTag",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "ExtTag",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ExtTag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewValue",
                table: "ExtTag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "ExtTag",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RID",
                table: "ExtTag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ValueType",
                table: "ExtTag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ServicesMonitor");

            migrationBuilder.DropColumn(
                name: "IPAdress",
                table: "ServicesMonitor");

            migrationBuilder.DropColumn(
                name: "IsConnected",
                table: "ServicesMonitor");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ServicesMonitor");

            migrationBuilder.DropColumn(
                name: "RID",
                table: "ServicesMonitor");

            migrationBuilder.DropColumn(
                name: "AnodeSize",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "IsMismatched",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "MeasuredType",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "CurrentValue",
                table: "ExtTag");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ExtTag");

            migrationBuilder.DropColumn(
                name: "HasNewValue",
                table: "ExtTag");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "ExtTag");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ExtTag");

            migrationBuilder.DropColumn(
                name: "NewValue",
                table: "ExtTag");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "ExtTag");

            migrationBuilder.DropColumn(
                name: "RID",
                table: "ExtTag");

            migrationBuilder.DropColumn(
                name: "ValueType",
                table: "ExtTag");
        }
    }
}
