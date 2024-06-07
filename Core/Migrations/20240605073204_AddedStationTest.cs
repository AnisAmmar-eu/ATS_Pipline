using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedStationTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StationTests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationID = table.Column<int>(type: "int", nullable: false),
                    AnodeType = table.Column<int>(type: "int", nullable: false),
                    SN_number = table.Column<int>(type: "int", nullable: false),
                    Photo1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Photo2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShootingTS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Cam1Status = table.Column<int>(type: "int", nullable: false),
                    Cam2Status = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationTests", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StationTests");
        }
    }
}
