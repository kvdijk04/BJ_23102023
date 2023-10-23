using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeSpecificEachProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "SizeSpecificEachProduct",
                columns: table => new
                {
                    SizeId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Energy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carbonhydrate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DietaryFibre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Protein = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatSaturated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CacbonhydrateSugar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sodium = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SizeSpecificEachProduct", x => new { x.ProductId, x.SizeId });
                    table.ForeignKey(
                        name: "FK_SizeSpecificEachProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SizeSpecificEachProduct_Size_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Size",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SizeSpecificEachProduct_SizeId",
                table: "SizeSpecificEachProduct",
                column: "SizeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "SizeSpecificProduct",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: false),
                    CacbonhydrateSugar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carbonhydrate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DietaryFibre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Energy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatSaturated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Protein = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sodium = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SizeSpecificProduct", x => new { x.ProductId, x.SizeId });
                    table.ForeignKey(
                        name: "FK_SizeSpecificProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SizeSpecificProduct_Size_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Size",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SizeSpecificProduct_SizeId",
                table: "SizeSpecificProduct",
                column: "SizeId");
        }
    }
}
