using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDayAndMonthInVisitorCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("e95eada5-1bc1-4394-9acc-d9280159db67"));

            migrationBuilder.DropColumn(
                name: "Count",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Count",
                table: "VisitorCounter",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Count", "Day", "Month", "Year" },
                values: new object[] { new Guid("e95eada5-1bc1-4394-9acc-d9280159db67"), 0L, 21, 11, 2023 });
        }
    }
}
