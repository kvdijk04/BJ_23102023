using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyInSizeSpecificProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CacbonhydrateSugar",
                table: "SizeSpecificProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Carbonhydrate",
                table: "SizeSpecificProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DietaryFibre",
                table: "SizeSpecificProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Energy",
                table: "SizeSpecificProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fat",
                table: "SizeSpecificProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatSaturated",
                table: "SizeSpecificProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Protein",
                table: "SizeSpecificProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sodium",
                table: "SizeSpecificProduct",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CacbonhydrateSugar",
                table: "SizeSpecificProduct");

            migrationBuilder.DropColumn(
                name: "Carbonhydrate",
                table: "SizeSpecificProduct");

            migrationBuilder.DropColumn(
                name: "DietaryFibre",
                table: "SizeSpecificProduct");

            migrationBuilder.DropColumn(
                name: "Energy",
                table: "SizeSpecificProduct");

            migrationBuilder.DropColumn(
                name: "Fat",
                table: "SizeSpecificProduct");

            migrationBuilder.DropColumn(
                name: "FatSaturated",
                table: "SizeSpecificProduct");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "SizeSpecificProduct");

            migrationBuilder.DropColumn(
                name: "Sodium",
                table: "SizeSpecificProduct");
        }
    }
}
