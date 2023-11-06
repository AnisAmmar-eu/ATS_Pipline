using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Added_Anodes_and_relations_to_stationCycles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anode",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    S1S2CycleRID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClosedTS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    S1S2CycleID = table.Column<int>(type: "int", nullable: false),
                    S3S4CycleID = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    S5CycleID = table.Column<int>(type: "int", nullable: true),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anode", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Anode_StationCycle_S1S2CycleID",
                        column: x => x.S1S2CycleID,
                        principalTable: "StationCycle",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Anode_StationCycle_S3S4CycleID",
                        column: x => x.S3S4CycleID,
                        principalTable: "StationCycle",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Anode_StationCycle_S5CycleID",
                        column: x => x.S5CycleID,
                        principalTable: "StationCycle",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anode_S1S2CycleID",
                table: "Anode",
                column: "S1S2CycleID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Anode_S3S4CycleID",
                table: "Anode",
                column: "S3S4CycleID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Anode_S5CycleID",
                table: "Anode",
                column: "S5CycleID",
                unique: true,
                filter: "[S5CycleID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anode");
        }
    }
}
