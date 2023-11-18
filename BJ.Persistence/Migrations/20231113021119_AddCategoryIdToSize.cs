using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Size",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Size_CategoryId",
                table: "Size",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Size_Category_CategoryId",
                table: "Size",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Size_Category_CategoryId",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_Size_CategoryId",
                table: "Size");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Size");
        }
    }
}
