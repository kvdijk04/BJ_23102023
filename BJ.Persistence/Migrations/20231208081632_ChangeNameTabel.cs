using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameTabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailConfigWebsite_ConfigWeb_ConfigWebId",
                table: "DetailConfigWebsite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfigWeb",
                table: "ConfigWeb");

            migrationBuilder.RenameTable(
                name: "ConfigWeb",
                newName: "ConfigWebsite");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfigWebsite",
                table: "ConfigWebsite",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailConfigWebsite_ConfigWebsite_ConfigWebId",
                table: "DetailConfigWebsite",
                column: "ConfigWebId",
                principalTable: "ConfigWebsite",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailConfigWebsite_ConfigWebsite_ConfigWebId",
                table: "DetailConfigWebsite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfigWebsite",
                table: "ConfigWebsite");

            migrationBuilder.RenameTable(
                name: "ConfigWebsite",
                newName: "ConfigWeb");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfigWeb",
                table: "ConfigWeb",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailConfigWebsite_ConfigWeb_ConfigWebId",
                table: "DetailConfigWebsite",
                column: "ConfigWebId",
                principalTable: "ConfigWeb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
