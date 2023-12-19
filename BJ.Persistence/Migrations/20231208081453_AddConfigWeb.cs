using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigWeb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VisitorCounter",
                keyColumn: "Id",
                keyValue: new Guid("05d73632-243f-4d09-ad09-2701b0dcb8d0"));

            migrationBuilder.CreateTable(
                name: "ConfigWeb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigWeb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetailConfigWebsite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigWebId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailConfigWebsite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailConfigWebsite_ConfigWeb_ConfigWebId",
                        column: x => x.ConfigWebId,
                        principalTable: "ConfigWeb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailConfigWebsiteTranslation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DetailConfigWebId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailConfigWebsiteTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailConfigWebsiteTranslation_DetailConfigWebsite_DetailConfigWebId",
                        column: x => x.DetailConfigWebId,
                        principalTable: "DetailConfigWebsite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailConfigWebsiteTranslation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailConfigWebsite_ConfigWebId",
                table: "DetailConfigWebsite",
                column: "ConfigWebId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailConfigWebsiteTranslation_DetailConfigWebId",
                table: "DetailConfigWebsiteTranslation",
                column: "DetailConfigWebId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailConfigWebsiteTranslation_LanguageId",
                table: "DetailConfigWebsiteTranslation",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailConfigWebsiteTranslation");

            migrationBuilder.DropTable(
                name: "DetailConfigWebsite");

            migrationBuilder.DropTable(
                name: "ConfigWeb");

            migrationBuilder.InsertData(
                table: "VisitorCounter",
                columns: new[] { "Id", "Day", "DayCount", "Month", "MonthCount", "Year", "YearCount" },
                values: new object[] { new Guid("05d73632-243f-4d09-ad09-2701b0dcb8d0"), 24, 0L, 11, 0L, 2023, 0L });
        }
    }
}
