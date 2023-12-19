using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveAndUrlToDetailConfigWeb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "DetailConfigWebsiteTranslation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "DetailConfigWebsite",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "DetailConfigWebsiteTranslation");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "DetailConfigWebsite");
        }
    }
}
