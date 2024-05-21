using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdCreatives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DisplayImageUrl = table.Column<int>(type: "INTEGER", nullable: false),
                    VideoUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Heading = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    OfferEnding = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HtmlString = table.Column<string>(type: "TEXT", nullable: true),
                    Discount = table.Column<double>(type: "REAL", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    AdvertLinkAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdCreatives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Advertisers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyPageViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Range = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyPageViews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sub = table.Column<string>(type: "TEXT", nullable: true),
                    NameIdentifier = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AdvertStyle = table.Column<string>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false),
                    SafetyComponents = table.Column<string>(type: "TEXT", nullable: false),
                    PaymentType = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetSites = table.Column<string>(type: "TEXT", nullable: false),
                    DailyBudget = table.Column<int>(type: "INTEGER", nullable: false),
                    DailyBudgetSet = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaximumDailyImpressions = table.Column<int>(type: "INTEGER", nullable: false),
                    MaximumDailyImpressionsSet = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaxBiddingAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxBiddingAmountSet = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AdCreativeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CampaignLengthInDays = table.Column<int>(type: "INTEGER", nullable: false),
                    ConversionsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AdvertLinkAddress = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaigns_AdCreatives_AdCreativeId",
                        column: x => x.AdCreativeId,
                        principalTable: "AdCreatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Campaigns_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredWebsites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Domain = table.Column<string>(type: "TEXT", nullable: false),
                    ExcludedPages = table.Column<string>(type: "TEXT", nullable: false),
                    AdsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasScriptTag = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredWebsites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisteredWebsites_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Clicks = table.Column<int>(type: "INTEGER", nullable: false),
                    CampaignId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Referrer = table.Column<string>(type: "TEXT", nullable: false),
                    PageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: false),
                    country = table.Column<string>(type: "TEXT", nullable: false),
                    duration = table.Column<int>(type: "INTEGER", nullable: false),
                    ConversionType = table.Column<int>(type: "INTEGER", nullable: false),
                    RegisteredWebsiteID = table.Column<int>(type: "INTEGER", nullable: false),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversion_RegisteredWebsites_RegisteredWebsiteID",
                        column: x => x.RegisteredWebsiteID,
                        principalTable: "RegisteredWebsites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conversion_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Impression",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Referrer = table.Column<string>(type: "TEXT", nullable: false),
                    PageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    RegisteredWebsiteID = table.Column<int>(type: "INTEGER", nullable: false),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impression", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impression_RegisteredWebsites_RegisteredWebsiteID",
                        column: x => x.RegisteredWebsiteID,
                        principalTable: "RegisteredWebsites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Impression_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_AdCreativeId",
                table: "Campaigns",
                column: "AdCreativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_UserId",
                table: "Campaigns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversion_RegisteredWebsiteID",
                table: "Conversion",
                column: "RegisteredWebsiteID");

            migrationBuilder.CreateIndex(
                name: "IX_Conversion_ReportId",
                table: "Conversion",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Impression_RegisteredWebsiteID",
                table: "Impression",
                column: "RegisteredWebsiteID");

            migrationBuilder.CreateIndex(
                name: "IX_Impression_ReportId",
                table: "Impression",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredWebsites_UserID",
                table: "RegisteredWebsites",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CampaignId",
                table: "Reports",
                column: "CampaignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisers");

            migrationBuilder.DropTable(
                name: "Conversion");

            migrationBuilder.DropTable(
                name: "Impression");

            migrationBuilder.DropTable(
                name: "MonthlyPageViews");

            migrationBuilder.DropTable(
                name: "RegisteredWebsites");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "AdCreatives");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
