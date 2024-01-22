using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreTranslation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "StoreLocation");

            migrationBuilder.DropColumn(
                name: "City",
                table: "StoreLocation");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "StoreLocation");

            migrationBuilder.CreateTable(
                name: "StoreLocationTranslation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreLocationId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreLocationTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreLocationTranslation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreLocationTranslation_StoreLocation_StoreLocationId",
                        column: x => x.StoreLocationId,
                        principalTable: "StoreLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocationTranslation_LanguageId",
                table: "StoreLocationTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocationTranslation_StoreLocationId",
                table: "StoreLocationTranslation",
                column: "StoreLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreLocationTranslation");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "StoreLocation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "StoreLocation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StoreLocation",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
