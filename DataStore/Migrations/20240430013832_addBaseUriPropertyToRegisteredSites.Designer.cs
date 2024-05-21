﻿// <auto-generated />
using System;
using DataStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataStore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240430013832_addBaseUriPropertyToRegisteredSites")]
    partial class addBaseUriPropertyToRegisteredSites
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("SharedModels.Models.AdCreative", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdvertLinkAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<double>("Discount")
                        .HasColumnType("REAL");

                    b.Property<int>("DisplayImageUrl")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Heading")
                        .HasColumnType("TEXT");

                    b.Property<int>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HtmlString")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("OfferEnding")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("VideoUrl")
                        .HasColumnType("TEXT");

                    b.Property<int>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("AdCreatives");
                });

            modelBuilder.Entity("SharedModels.Models.Advertiser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Advertisers");
                });

            modelBuilder.Entity("SharedModels.Models.Campaign", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AdCreativeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdvertLinkAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("AdvertStyle")
                        .HasColumnType("TEXT");

                    b.Property<int>("CampaignLengthInDays")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ConversionsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int>("DailyBudget")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("DailyBudgetSet")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxBiddingAmount")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("MaxBiddingAmountSet")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaximumDailyImpressions")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("MaximumDailyImpressionsSet")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Orientation")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PaymentType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SafetyComponents")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .HasColumnType("TEXT");

                    b.Property<string>("TargetSites")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AdCreativeId");

                    b.HasIndex("UserId");

                    b.ToTable("Campaigns");
                });

            modelBuilder.Entity("SharedModels.Models.Conversion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConversionType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Referrer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RegisteredWebsiteID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ReportId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SessionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("country")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("duration")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RegisteredWebsiteID");

                    b.HasIndex("ReportId");

                    b.ToTable("Conversion");
                });

            modelBuilder.Entity("SharedModels.Models.Impression", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Referrer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RegisteredWebsiteID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ReportId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SessionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RegisteredWebsiteID");

                    b.HasIndex("ReportId");

                    b.ToTable("Impression");
                });

            modelBuilder.Entity("SharedModels.Models.MonthlyPageView", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Range")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MonthlyPageViews");
                });

            modelBuilder.Entity("SharedModels.Models.RegisteredWebsite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AdsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Audience")
                        .HasColumnType("TEXT");

                    b.Property<string>("BaseUri")
                        .HasColumnType("TEXT");

                    b.Property<string>("Categories")
                        .HasColumnType("TEXT");

                    b.Property<string>("Continent")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExcludedPages")
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasScriptTag")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TagHashCode")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserID");

                    b.ToTable("RegisteredWebsites");
                });

            modelBuilder.Entity("SharedModels.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CampaignId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Clicks")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("SharedModels.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("NameIdentifier")
                        .HasColumnType("TEXT");

                    b.Property<string>("Sub")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SharedModels.Models.Campaign", b =>
                {
                    b.HasOne("SharedModels.Models.AdCreative", "AdCreative")
                        .WithMany()
                        .HasForeignKey("AdCreativeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SharedModels.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdCreative");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SharedModels.Models.Conversion", b =>
                {
                    b.HasOne("SharedModels.Models.RegisteredWebsite", "RegisteredWebsite")
                        .WithMany()
                        .HasForeignKey("RegisteredWebsiteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SharedModels.Models.Report", null)
                        .WithMany("Conversions")
                        .HasForeignKey("ReportId");

                    b.Navigation("RegisteredWebsite");
                });

            modelBuilder.Entity("SharedModels.Models.Impression", b =>
                {
                    b.HasOne("SharedModels.Models.RegisteredWebsite", "RegisteredWebsite")
                        .WithMany()
                        .HasForeignKey("RegisteredWebsiteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SharedModels.Models.Report", null)
                        .WithMany("Impressions")
                        .HasForeignKey("ReportId");

                    b.Navigation("RegisteredWebsite");
                });

            modelBuilder.Entity("SharedModels.Models.RegisteredWebsite", b =>
                {
                    b.HasOne("SharedModels.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SharedModels.Models.Report", b =>
                {
                    b.HasOne("SharedModels.Models.Campaign", "Campaign")
                        .WithMany()
                        .HasForeignKey("CampaignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Campaign");
                });

            modelBuilder.Entity("SharedModels.Models.Report", b =>
                {
                    b.Navigation("Conversions");

                    b.Navigation("Impressions");
                });
#pragma warning restore 612, 618
        }
    }
}