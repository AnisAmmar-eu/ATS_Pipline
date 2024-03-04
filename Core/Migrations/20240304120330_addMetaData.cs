using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class addMetaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anode_StationCycle_S1S2CycleID",
                table: "Anode");

            migrationBuilder.DropForeignKey(
                name: "FK_StationCycle_Packet_ShootingID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_ShootingID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_Anode_S1S2CycleID",
                table: "Anode");

            migrationBuilder.RenameColumn(
                name: "ShootingStatus",
                table: "StationCycle",
                newName: "Shooting2ID");

            migrationBuilder.RenameColumn(
                name: "ShootingID",
                table: "StationCycle",
                newName: "Shooting1ID");

            migrationBuilder.RenameColumn(
                name: "AlarmListStatus",
                table: "StationCycle",
                newName: "MetaDataID");

            migrationBuilder.RenameColumn(
                name: "S1S2CycleRID",
                table: "Anode",
                newName: "CycleRID");

            migrationBuilder.AddColumn<int>(
                name: "Picture1Status",
                table: "StationCycle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Picture2Status",
                table: "StationCycle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "AnodeSize",
                table: "Packet",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnodeTypeStatus",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnodeType_MD",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnodeType_RW",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Cam01Temp",
                table: "Packet",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Cam02Temp",
                table: "Packet",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Double_RW",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SN_Day",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SN_Month",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SN_Number",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SN_StationID",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SN_Vibro",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SN_Year",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shooting_Cam01Status",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shooting_Cam02Status",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SyncIndex",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SyncIndex_RW",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TT01",
                table: "Packet",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Trolley",
                table: "Packet",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "S1S2CycleTS",
                table: "Anode",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<int>(
                name: "S1S2CycleStationID",
                table: "Anode",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "S1S2CycleID",
                table: "Anode",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_MetaDataID",
                table: "StationCycle",
                column: "MetaDataID",
                unique: true,
                filter: "[MetaDataID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_Shooting1ID",
                table: "StationCycle",
                column: "Shooting1ID",
                unique: true,
                filter: "[Shooting1ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_Shooting2ID",
                table: "StationCycle",
                column: "Shooting2ID",
                unique: true,
                filter: "[Shooting2ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Anode_S1S2CycleID",
                table: "Anode",
                column: "S1S2CycleID",
                unique: true,
                filter: "[S1S2CycleID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Anode_StationCycle_S1S2CycleID",
                table: "Anode",
                column: "S1S2CycleID",
                principalTable: "StationCycle",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_StationCycle_Packet_MetaDataID",
                table: "StationCycle",
                column: "MetaDataID",
                principalTable: "Packet",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_StationCycle_Packet_Shooting1ID",
                table: "StationCycle",
                column: "Shooting1ID",
                principalTable: "Packet",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_StationCycle_Packet_Shooting2ID",
                table: "StationCycle",
                column: "Shooting2ID",
                principalTable: "Packet",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anode_StationCycle_S1S2CycleID",
                table: "Anode");

            migrationBuilder.DropForeignKey(
                name: "FK_StationCycle_Packet_MetaDataID",
                table: "StationCycle");

            migrationBuilder.DropForeignKey(
                name: "FK_StationCycle_Packet_Shooting1ID",
                table: "StationCycle");

            migrationBuilder.DropForeignKey(
                name: "FK_StationCycle_Packet_Shooting2ID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_MetaDataID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_Shooting1ID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_StationCycle_Shooting2ID",
                table: "StationCycle");

            migrationBuilder.DropIndex(
                name: "IX_Anode_S1S2CycleID",
                table: "Anode");

            migrationBuilder.DropColumn(
                name: "Picture1Status",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "Picture2Status",
                table: "StationCycle");

            migrationBuilder.DropColumn(
                name: "AnodeSize",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "AnodeTypeStatus",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "AnodeType_MD",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "AnodeType_RW",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Cam01Temp",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Cam02Temp",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Double_RW",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SN_Day",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SN_Month",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SN_Number",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SN_StationID",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SN_Vibro",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SN_Year",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Shooting_Cam01Status",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Shooting_Cam02Status",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SyncIndex",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SyncIndex_RW",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "TT01",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Trolley",
                table: "Packet");

            migrationBuilder.RenameColumn(
                name: "Shooting2ID",
                table: "StationCycle",
                newName: "ShootingStatus");

            migrationBuilder.RenameColumn(
                name: "Shooting1ID",
                table: "StationCycle",
                newName: "ShootingID");

            migrationBuilder.RenameColumn(
                name: "MetaDataID",
                table: "StationCycle",
                newName: "AlarmListStatus");

            migrationBuilder.RenameColumn(
                name: "CycleRID",
                table: "Anode",
                newName: "S1S2CycleRID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "S1S2CycleTS",
                table: "Anode",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "S1S2CycleStationID",
                table: "Anode",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "S1S2CycleID",
                table: "Anode",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationCycle_ShootingID",
                table: "StationCycle",
                column: "ShootingID",
                unique: true,
                filter: "[ShootingID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Anode_S1S2CycleID",
                table: "Anode",
                column: "S1S2CycleID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Anode_StationCycle_S1S2CycleID",
                table: "Anode",
                column: "S1S2CycleID",
                principalTable: "StationCycle",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StationCycle_Packet_ShootingID",
                table: "StationCycle",
                column: "ShootingID",
                principalTable: "Packet",
                principalColumn: "ID");
        }
    }
}
