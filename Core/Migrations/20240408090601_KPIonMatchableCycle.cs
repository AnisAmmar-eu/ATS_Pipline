using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class KPIonMatchableCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SANfile",
                table: "ToUnload");

            migrationBuilder.DropColumn(
                name: "SANfile",
                table: "Dataset");

            migrationBuilder.AddColumn<int>(
                name: "InstanceMatchID",
                table: "ToUnload",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstanceMatchID",
                table: "ToMatch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "StationCycle",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstanceMatchID",
                table: "ToUnload");

            migrationBuilder.DropColumn(
                name: "InstanceMatchID",
                table: "ToMatch");

            migrationBuilder.AddColumn<string>(
                name: "SANfile",
                table: "ToUnload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "StationCycle",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.AddColumn<string>(
                name: "SANfile",
                table: "Dataset",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
