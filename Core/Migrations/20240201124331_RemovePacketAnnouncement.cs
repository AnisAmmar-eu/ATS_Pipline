using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class RemovePacketAnnouncement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationCycle_Packet_AnnouncementID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_AnnouncementID",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "AnnouncementID",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "AnnouncementStatus",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Shooting_AnodeType",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Shooting_SyncIndex",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "TrolleyNumber",
                table: "Packet");

            migrationBuilder.RenameColumn(
                name: "IsDouble",
                table: "Packet",
                newName: "S5Shooting_IsDoubleAnode");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Packet",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "S5Shooting_IsDoubleAnode",
                table: "Packet",
                newName: "IsDouble");

            migrationBuilder.AddColumn<int>(
                name: "AnnouncementID",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnnouncementStatus",
                table: "StationCycle",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Packet",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shooting_AnodeType",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shooting_SyncIndex",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrolleyNumber",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_AnnouncementID",
                table: "StationCycle",
                column: "AnnouncementID",
                unique: true,
                filter: "[AnnouncementID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_StationCycle_Packet_AnnouncementID",
                table: "StationCycle",
                column: "AnnouncementID",
                principalTable: "Packet",
                principalColumn: "ID");
        }
    }
}
