using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangedInstanceMatchID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileSetting");

            migrationBuilder.RenameColumn(
                name: "DataSetID",
                table: "ToLoad",
                newName: "InstanceMatchID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InstanceMatchID",
                table: "ToLoad",
                newName: "DataSetID");

            migrationBuilder.CreateTable(
                name: "FileSetting",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModification = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUploadName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TS = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSetting", x => x.ID);
                });
        }
    }
}
