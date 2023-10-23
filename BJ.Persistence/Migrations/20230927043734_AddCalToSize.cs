using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCalToSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CacbonhydrateSugar",
                table: "SizeSpecificEachProduct",
                newName: "CarbonhydrateSugar");

            migrationBuilder.AddColumn<string>(
                name: "Cal",
                table: "SizeSpecificEachProduct",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cal",
                table: "SizeSpecificEachProduct");

            migrationBuilder.RenameColumn(
                name: "CarbonhydrateSugar",
                table: "SizeSpecificEachProduct",
                newName: "CacbonhydrateSugar");
        }
    }
}
