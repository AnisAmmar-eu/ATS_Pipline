using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AlarmC_NewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BIPeriodicLog");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "AlarmC",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                table: "AlarmC",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "AlarmC");

            migrationBuilder.DropColumn(
                name: "Severity",
                table: "AlarmC");

            migrationBuilder.CreateTable(
                name: "BIPeriodicLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiveAlarms = table.Column<int>(type: "int", nullable: false),
                    Cam1Matched = table.Column<int>(type: "int", nullable: false),
                    Cam2Matched = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InactiveAlarms = table.Column<int>(type: "int", nullable: false),
                    NbAnodeS1 = table.Column<int>(type: "int", nullable: false),
                    NbAnodeS2 = table.Column<int>(type: "int", nullable: false),
                    NbAnodeS3 = table.Column<int>(type: "int", nullable: false),
                    NbAnodeS4 = table.Column<int>(type: "int", nullable: false),
                    NbAnodeS5 = table.Column<int>(type: "int", nullable: false),
                    NbMatched = table.Column<int>(type: "int", nullable: false),
                    NbSigned = table.Column<int>(type: "int", nullable: false),
                    NbUnsigned = table.Column<int>(type: "int", nullable: false),
                    NonAckAlarms = table.Column<int>(type: "int", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BIPeriodicLog", x => x.ID);
                });
        }
    }
}
