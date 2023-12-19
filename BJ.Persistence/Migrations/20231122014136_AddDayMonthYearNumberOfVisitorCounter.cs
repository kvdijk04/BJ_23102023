using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDayMonthYearNumberOfVisitorCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("dc8ed870-53eb-4133-a243-7a4ff69b73eb"));

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "VisitorCounter",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Month",
                table: "VisitorCounter",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Day",
                table: "VisitorCounter",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "DayCount",
                table: "VisitorCounter",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MonthCount",
                table: "VisitorCounter",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "YearCount",
                table: "VisitorCounter",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Day", "DayCount", "Month", "MonthCount", "Year", "YearCount" },
                values: new object[] { new Guid("6a049d01-7f3b-4523-82c6-a89cb58285d6"), 22, 0L, 11, 0L, 2023, 0L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("6a049d01-7f3b-4523-82c6-a89cb58285d6"));

            migrationBuilder.DropColumn(
                name: "DayCount",
                table: "VisitorCounter");

            migrationBuilder.DropColumn(
                name: "MonthCount",
                table: "VisitorCounter");

            migrationBuilder.DropColumn(
                name: "YearCount",
                table: "VisitorCounter");

            migrationBuilder.AlterColumn<long>(
                name: "Year",
                table: "VisitorCounter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Month",
                table: "VisitorCounter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Day",
                table: "VisitorCounter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Day", "Month", "Year" },
                values: new object[] { new Guid("dc8ed870-53eb-4133-a243-7a4ff69b73eb"), 0L, 0L, 0L });
        }
    }
}
