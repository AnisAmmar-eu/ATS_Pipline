using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Packages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Packet",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CycleStationRID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Placeholder = table.Column<int>(type: "int", nullable: true),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AlarmCycle",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IRID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlarmRID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Station = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NbNonAck = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TSRaised = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TSClear = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AlarmListPacketID = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmCycle", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AlarmCycle_Packet_AlarmListPacketID",
                        column: x => x.AlarmListPacketID,
                        principalTable: "Packet",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlarmCycle_AlarmListPacketID",
                table: "AlarmCycle",
                column: "AlarmListPacketID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlarmCycle");

            migrationBuilder.DropTable(
                name: "Packet");
        }
    }
}
