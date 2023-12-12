using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedLoadableQueueAndMatchableStack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CycleTS",
                table: "MatchableStack",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "DataSetID",
                table: "MatchableStack",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CycleTS",
                table: "LoadableQueue",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "DataSetID",
                table: "LoadableQueue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MatchableStack_CycleTS",
                table: "MatchableStack",
                column: "CycleTS");

            migrationBuilder.CreateIndex(
                name: "IX_LoadableQueue_CycleTS",
                table: "LoadableQueue",
                column: "CycleTS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MatchableStack_CycleTS",
                table: "MatchableStack");

            migrationBuilder.DropIndex(
                name: "IX_LoadableQueue_CycleTS",
                table: "LoadableQueue");

            migrationBuilder.DropColumn(
                name: "CycleTS",
                table: "MatchableStack");

            migrationBuilder.DropColumn(
                name: "DataSetID",
                table: "MatchableStack");

            migrationBuilder.DropColumn(
                name: "CycleTS",
                table: "LoadableQueue");

            migrationBuilder.DropColumn(
                name: "DataSetID",
                table: "LoadableQueue");
        }
    }
}
