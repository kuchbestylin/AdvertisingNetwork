using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class EditReportStructureAddViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversion_RegisteredWebsites_RegisteredWebsiteID",
                table: "Conversion");

            migrationBuilder.DropForeignKey(
                name: "FK_Impression_Reports_ReportId",
                table: "Impression");

            migrationBuilder.DropIndex(
                name: "IX_Impression_ReportId",
                table: "Impression");

            migrationBuilder.DropIndex(
                name: "IX_Conversion_RegisteredWebsiteID",
                table: "Conversion");

            migrationBuilder.DropColumn(
                name: "Clicks",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "Impression");

            migrationBuilder.DropColumn(
                name: "ConversionType",
                table: "Conversion");

            migrationBuilder.DropColumn(
                name: "RegisteredWebsiteID",
                table: "Conversion");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Conversion");

            migrationBuilder.DropColumn(
                name: "duration",
                table: "Conversion");

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "Conversion",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "Conversion",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Referrer",
                table: "Conversion",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "PageUrl",
                table: "Conversion",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "Conversion",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Conversion",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Click",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Click", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Click_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Hover",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Duration = table.Column<double>(type: "REAL", nullable: false),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hover", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hover_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "View",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_View", x => x.Id);
                    table.ForeignKey(
                        name: "FK_View_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Click_ReportId",
                table: "Click",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Hover_ReportId",
                table: "Hover",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_View_ReportId",
                table: "View",
                column: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Click");

            migrationBuilder.DropTable(
                name: "Hover");

            migrationBuilder.DropTable(
                name: "View");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Conversion");

            migrationBuilder.AddColumn<int>(
                name: "Clicks",
                table: "Reports",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "Impression",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "Conversion",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "Conversion",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Referrer",
                table: "Conversion",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PageUrl",
                table: "Conversion",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "Conversion",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConversionType",
                table: "Conversion",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegisteredWebsiteID",
                table: "Conversion",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "Conversion",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "duration",
                table: "Conversion",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Impression_ReportId",
                table: "Impression",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversion_RegisteredWebsiteID",
                table: "Conversion",
                column: "RegisteredWebsiteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversion_RegisteredWebsites_RegisteredWebsiteID",
                table: "Conversion",
                column: "RegisteredWebsiteID",
                principalTable: "RegisteredWebsites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Impression_Reports_ReportId",
                table: "Impression",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");
        }
    }
}
