using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("6a049d01-7f3b-4523-82c6-a89cb58285d6"));

            migrationBuilder.AddColumn<string>(
                name: "AuthorizeRole",
                table: "Account",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Day", "DayCount", "Month", "MonthCount", "Year", "YearCount" },
                values: new object[] { new Guid("e75bb19a-ac66-4892-8832-9eaf2420aad5"), 23, 0L, 11, 0L, 2023, 0L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("e75bb19a-ac66-4892-8832-9eaf2420aad5"));

            migrationBuilder.DropColumn(
                name: "AuthorizeRole",
                table: "Account");

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Day", "DayCount", "Month", "MonthCount", "Year", "YearCount" },
                values: new object[] { new Guid("6a049d01-7f3b-4523-82c6-a89cb58285d6"), 22, 0L, 11, 0L, 2023, 0L });
        }
    }
}
