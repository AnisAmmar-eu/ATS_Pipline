using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedMatchableStackAndLoadableQueue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "StationCycle",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.CreateTable(
                name: "LoadableQueue",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadableCycleID = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SAN1Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAN2Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadableQueue", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LoadableQueue_StationCycle_LoadableCycleID",
                        column: x => x.LoadableCycleID,
                        principalTable: "StationCycle",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchableStack",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchableCycleID = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SAN1Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAN2Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchableStack", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MatchableStack_StationCycle_MatchableCycleID",
                        column: x => x.MatchableCycleID,
                        principalTable: "StationCycle",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadableQueue_LoadableCycleID",
                table: "LoadableQueue",
                column: "LoadableCycleID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchableStack_MatchableCycleID",
                table: "MatchableStack",
                column: "MatchableCycleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadableQueue");

            migrationBuilder.DropTable(
                name: "MatchableStack");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "StationCycle",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);
        }
    }
}
