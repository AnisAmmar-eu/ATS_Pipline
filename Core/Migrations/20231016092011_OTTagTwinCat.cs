using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class OTTagTwinCat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtTag");

            migrationBuilder.DropTable(
                name: "ServicesMonitor");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "IOTTag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ValueType",
                table: "IOTTag",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "IOTTag");

            migrationBuilder.DropColumn(
                name: "ValueType",
                table: "IOTTag");

            migrationBuilder.CreateTable(
                name: "ServicesMonitor",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsConnected = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesMonitor", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ExtTag",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    CurrentValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasNewValue = table.Column<bool>(type: "bit", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ValueType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtTag", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExtTag_ServicesMonitor_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "ServicesMonitor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtTag_ServiceID",
                table: "ExtTag",
                column: "ServiceID");
        }
    }
}
