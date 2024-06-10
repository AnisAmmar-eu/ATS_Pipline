using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchingResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ChainMatchingResult",
                table: "StationCycle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MatchingResult",
                table: "StationCycle",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChainMatchingResult",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "MatchingResult",
                table: "StationCycle");
        }
    }
}
