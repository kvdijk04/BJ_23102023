using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Config : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("e75bb19a-ac66-4892-8832-9eaf2420aad5"));

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Day", "DayCount", "Month", "MonthCount", "Year", "YearCount" },
                values: new object[] { new Guid("55e6610a-08ec-4311-87a2-dafb114dc40b"), 23, 0L, 11, 0L, 2023, 0L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("55e6610a-08ec-4311-87a2-dafb114dc40b"));

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Day", "DayCount", "Month", "MonthCount", "Year", "YearCount" },
                values: new object[] { new Guid("e75bb19a-ac66-4892-8832-9eaf2420aad5"), 23, 0L, 11, 0L, 2023, 0L });
        }
    }
}
