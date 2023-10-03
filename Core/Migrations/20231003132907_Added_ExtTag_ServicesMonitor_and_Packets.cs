using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Added_ExtTag_ServicesMonitor_and_Packets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Placeholder",
                table: "Packet");

            migrationBuilder.RenameColumn(
                name: "PacketType",
                table: "Packet",
                newName: "Type");

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Server = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Api = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Function = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Endpoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ServicesMonitor",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtTag");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "ServicesMonitor");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Packet",
                newName: "PacketType");

            migrationBuilder.AddColumn<int>(
                name: "Placeholder",
                table: "Packet",
                type: "int",
                nullable: true);
        }
    }
}
