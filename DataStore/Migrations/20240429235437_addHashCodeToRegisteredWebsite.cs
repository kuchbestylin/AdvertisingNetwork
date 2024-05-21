using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addHashCodeToRegisteredWebsite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExcludedPages",
                table: "RegisteredWebsites",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Audience",
                table: "RegisteredWebsites",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "RegisteredWebsites",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Continent",
                table: "RegisteredWebsites",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RegisteredWebsites",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TagHashCode",
                table: "RegisteredWebsites",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Audience",
                table: "RegisteredWebsites");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "RegisteredWebsites");

            migrationBuilder.DropColumn(
                name: "Continent",
                table: "RegisteredWebsites");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RegisteredWebsites");

            migrationBuilder.DropColumn(
                name: "TagHashCode",
                table: "RegisteredWebsites");

            migrationBuilder.AlterColumn<string>(
                name: "ExcludedPages",
                table: "RegisteredWebsites",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
