using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace DataStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
        }

        public DbSet<AdCreative> AdCreatives { get; set; }
        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<MonthlyPageView> MonthlyPageViews { get; set; }
        public DbSet<RegisteredWebsite> RegisteredWebsites { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Impression> Impressions { get; set; }
        public DbSet<Impression> Conversions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData();
        }
    }
}
