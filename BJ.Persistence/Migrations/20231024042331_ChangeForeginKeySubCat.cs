using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeForeginKeySubCat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategoryTranslation_Language_SubCatName",
                table: "SubCategoryTranslation");

            migrationBuilder.DropIndex(
                name: "IX_SubCategoryTranslation_SubCatName",
                table: "SubCategoryTranslation");

            migrationBuilder.AlterColumn<string>(
                name: "SubCatName",
                table: "SubCategoryTranslation",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageId",
                table: "SubCategoryTranslation",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategoryTranslation_LanguageId",
                table: "SubCategoryTranslation",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategoryTranslation_Language_LanguageId",
                table: "SubCategoryTranslation",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategoryTranslation_Language_LanguageId",
                table: "SubCategoryTranslation");

            migrationBuilder.DropIndex(
                name: "IX_SubCategoryTranslation_LanguageId",
                table: "SubCategoryTranslation");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "SubCategoryTranslation");

            migrationBuilder.AlterColumn<string>(
                name: "SubCatName",
                table: "SubCategoryTranslation",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategoryTranslation_SubCatName",
                table: "SubCategoryTranslation",
                column: "SubCatName");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategoryTranslation_Language_SubCatName",
                table: "SubCategoryTranslation",
                column: "SubCatName",
                principalTable: "Language",
                principalColumn: "Id");
        }
    }
}
