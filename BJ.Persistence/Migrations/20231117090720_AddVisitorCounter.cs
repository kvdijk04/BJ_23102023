using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitorCounter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorCounter", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Count", "Year" },
                values: new object[] { new Guid("28617664-5332-49c8-99d6-699393b4cc1e"), 0L, 2023 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitorCounter");
        }
    }
}
