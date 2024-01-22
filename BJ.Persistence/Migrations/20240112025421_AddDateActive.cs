using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BJ.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDateActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateActiveForm",
                table: "Product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeActiveTo",
                table: "Product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateActiveForm",
                table: "News",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeActiveTo",
                table: "News",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateActiveForm",
                table: "Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeActiveTo",
                table: "Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateActiveForm",
                table: "Blog",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeActiveTo",
                table: "Blog",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateActiveForm",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DateTimeActiveTo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DateActiveForm",
                table: "News");

            migrationBuilder.DropColumn(
                name: "DateTimeActiveTo",
                table: "News");

            migrationBuilder.DropColumn(
                name: "DateActiveForm",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DateTimeActiveTo",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DateActiveForm",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "DateTimeActiveTo",
                table: "Blog");
        }
    }
}
