using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddImageStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("951b80c3-1e91-43a7-9d4b-71b9beb93ac9"));

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "StoreLocation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Day", "DayCount", "Month", "MonthCount", "Year", "YearCount" },
                values: new object[] { new Guid("05d73632-243f-4d09-ad09-2701b0dcb8d0"), 24, 0L, 11, 0L, 2023, 0L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("05d73632-243f-4d09-ad09-2701b0dcb8d0"));

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "StoreLocation");

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Day", "DayCount", "Month", "MonthCount", "Year", "YearCount" },
                values: new object[] { new Guid("951b80c3-1e91-43a7-9d4b-71b9beb93ac9"), 23, 0L, 11, 0L, 2023, 0L });
        }
    }
}
