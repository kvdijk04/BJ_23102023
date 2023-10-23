using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveToSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "SizeSpecificEachProduct",
                newName: "ActiveNutri");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Size",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Size");

            migrationBuilder.RenameColumn(
                name: "ActiveNutri",
                table: "SizeSpecificEachProduct",
                newName: "Active");
        }
    }
}
