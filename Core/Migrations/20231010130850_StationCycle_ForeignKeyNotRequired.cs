using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class StationCycle_ForeignKeyNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StationCycle_AlarmListID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_AnnouncementID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_DetectionID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_ShootingID",
                table: "StationCycle");

            migrationBuilder.AlterColumn<int>(
                name: "ShootingID",
                table: "StationCycle",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DetectionID",
                table: "StationCycle",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AnnouncementID",
                table: "StationCycle",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AlarmListID",
                table: "StationCycle",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_AlarmListID",
                table: "StationCycle",
                column: "AlarmListID",
                unique: true,
                filter: "[AlarmListID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_AnnouncementID",
                table: "StationCycle",
                column: "AnnouncementID",
                unique: true,
                filter: "[AnnouncementID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_DetectionID",
                table: "StationCycle",
                column: "DetectionID",
                unique: true,
                filter: "[DetectionID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_ShootingID",
                table: "StationCycle",
                column: "ShootingID",
                unique: true,
                filter: "[ShootingID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StationCycle_AlarmListID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_AnnouncementID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_DetectionID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_ShootingID",
                table: "StationCycle");

            migrationBuilder.AlterColumn<int>(
                name: "ShootingID",
                table: "StationCycle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DetectionID",
                table: "StationCycle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AnnouncementID",
                table: "StationCycle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AlarmListID",
                table: "StationCycle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_AlarmListID",
                table: "StationCycle",
                column: "AlarmListID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_AnnouncementID",
                table: "StationCycle",
                column: "AnnouncementID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_DetectionID",
                table: "StationCycle",
                column: "DetectionID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_ShootingID",
                table: "StationCycle",
                column: "ShootingID",
                unique: true);
        }
    }
}
