using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDayAndMonth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("28617664-5332-49c8-99d6-699393b4cc1e"));

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "VisitorCounter",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "VisitorCounter",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Count", "Day", "Month", "Year" },
                values: new object[] { new Guid("e95eada5-1bc1-4394-9acc-d9280159db67"), 0L, 21, 11, 2023 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("e95eada5-1bc1-4394-9acc-d9280159db67"));

            migrationBuilder.DropColumn(
                name: "Day",
                table: "VisitorCounter");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "VisitorCounter");

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Count", "Year" },
                values: new object[] { new Guid("28617664-5332-49c8-99d6-699393b4cc1e"), 0L, 2023 });
        }
    }
}
