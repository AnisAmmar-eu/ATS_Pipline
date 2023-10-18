using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class S1S2Cycle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PITSectionNumber",
                table: "Packet",
                newName: "PitSectionNumber");

            migrationBuilder.RenameColumn(
                name: "PITNumber",
                table: "Packet",
                newName: "PitNumber");

            migrationBuilder.RenameColumn(
                name: "PITHeight",
                table: "Packet",
                newName: "PitHeight");

            migrationBuilder.RenameColumn(
                name: "TSUnpackPIT",
                table: "Packet",
                newName: "PitLoadTS");

            migrationBuilder.RenameColumn(
                name: "TSLoad",
                table: "Packet",
                newName: "PickUpTS");

            migrationBuilder.RenameColumn(
                name: "TSCentralConveyor",
                table: "Packet",
                newName: "DepositTS");

            migrationBuilder.RenameColumn(
                name: "GreenPosition",
                table: "Packet",
                newName: "PackPosition");

            migrationBuilder.RenameColumn(
                name: "FTAinPIT",
                table: "Packet",
                newName: "InvalidPacket");

            migrationBuilder.RenameColumn(
                name: "FTASuckPit",
                table: "Packet",
                newName: "GreenConvPos");

            migrationBuilder.RenameColumn(
                name: "BakedPosition",
                table: "Packet",
                newName: "FTASuck");

            migrationBuilder.RenameColumn(
                name: "AnodePosition",
                table: "Packet",
                newName: "FTAPlace");

            migrationBuilder.AddColumn<string>(
                name: "AnnounceID",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BakedConvPos",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InFurnace_AnnounceID",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnounceID",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "BakedConvPos",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "InFurnace_AnnounceID",
                table: "Packet");

            migrationBuilder.RenameColumn(
                name: "PitSectionNumber",
                table: "Packet",
                newName: "PITSectionNumber");

            migrationBuilder.RenameColumn(
                name: "PitNumber",
                table: "Packet",
                newName: "PITNumber");

            migrationBuilder.RenameColumn(
                name: "PitHeight",
                table: "Packet",
                newName: "PITHeight");

            migrationBuilder.RenameColumn(
                name: "PitLoadTS",
                table: "Packet",
                newName: "TSUnpackPIT");

            migrationBuilder.RenameColumn(
                name: "PickUpTS",
                table: "Packet",
                newName: "TSLoad");

            migrationBuilder.RenameColumn(
                name: "PackPosition",
                table: "Packet",
                newName: "GreenPosition");

            migrationBuilder.RenameColumn(
                name: "InvalidPacket",
                table: "Packet",
                newName: "FTAinPIT");

            migrationBuilder.RenameColumn(
                name: "GreenConvPos",
                table: "Packet",
                newName: "FTASuckPit");

            migrationBuilder.RenameColumn(
                name: "FTASuck",
                table: "Packet",
                newName: "BakedPosition");

            migrationBuilder.RenameColumn(
                name: "FTAPlace",
                table: "Packet",
                newName: "AnodePosition");

            migrationBuilder.RenameColumn(
                name: "DepositTS",
                table: "Packet",
                newName: "TSCentralConveyor");
        }
    }
}
