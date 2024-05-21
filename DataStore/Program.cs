using DataStore;
using DataStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using SharedModels.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.AddRedisClient(builder.Configuration["RedisCache"]!);

builder.Services.AddRedisCache();

builder.Services.AddAuthentication();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .WithMethods("GET", "POST");
        });
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseCors();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
    // Create the folder if it does not exist
    Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "Uploads")).FullName),
    RequestPath = "/Uploads"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/baseuri", (IHttpContextAccessor httpContextAccessor) =>
{
    return httpContextAccessor.HttpContext?.Request.Host;
});

app.MapPost("/users", async (
    [FromBody] User user,
    [FromServices] ApplicationDbContext dbContext,
    [FromServices] RedisService redisCache) =>
{
    if(user != null)
    {
        var usr = await redisCache.GetUserAsync<User>(user.Sub!);

        if (usr != null)
            return Results.Ok(usr);

        usr = dbContext.Users.FirstOrDefault(u => u.Sub == user.Sub);

        if (usr is null)
        {
            usr = dbContext.Users.Add(user).Entity;
            dbContext.SaveChanges();
        }

        await redisCache.SaveUserAsync(
        key: usr.Sub!,
        value: usr,
        expiry: TimeSpan.FromDays(30));

        return Results.Created("", usr);
    }

    return Results.BadRequest(user);
})
.WithName("PostUser")
.Accepts<string>("application/json")
.Produces(StatusCodes.Status400BadRequest, responseType: typeof(User))
.Produces(StatusCodes.Status400BadRequest, responseType: typeof(String))
.Produces(StatusCodes.Status201Created)
.WithOpenApi(x => new OpenApiOperation(x)
{
    Summary = "Creates a Local User",
    Description = "Create a new Local User using identifiers from Identity Server.",
    Tags = new List<OpenApiTag> { new() { Name = "User Store" } }
});

app.MapGet("/users/{sub}", (
    string sub,
    ApplicationDbContext dbContext) =>
{
    if(sub is not null)
    {
        var user = dbContext.Users.First(u => u.Sub.Equals(u));
        if (user is not null)
            return Results.Ok(user);
    }

    return Results.NotFound(sub);
}).WithName("GetUser")
.Accepts<string>("application/json")
.Produces(StatusCodes.Status400BadRequest, responseType: typeof(User))
.Produces(StatusCodes.Status400BadRequest, responseType: typeof(String))
.Produces(StatusCodes.Status201Created)
.WithOpenApi(x => new OpenApiOperation(x)
{
    Summary = "Gets a Local User",
    Description = "Create a Local version of the User using the Users identifier from Identity Server.",
    Tags = new List<OpenApiTag> { new() { Name = "User Store" } }
});

var sellside = app.MapGroup("sellside");

app.MapPost("/file", async (HttpRequest request, ApplicationDbContext db) => 
{
    if (!request.HasFormContentType)
        return Results.BadRequest("Request does not contain form data");

    var form = await request.ReadFormAsync();
    var campaignData = form["campaign"];
    var file = form.Files["image"];

    if (file is null || file.Length == 0)
        return Results.BadRequest("Image file is missing");

    var fileExtension = Path.GetExtension(file.FileName);
    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
    if (!Directory.Exists(uploadFolder))
    {
        Directory.CreateDirectory(uploadFolder);
    }

    var fileName = $"{Guid.NewGuid()}{fileExtension}";
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.FileName);
    using (var stream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    var campaign = JsonSerializer.Deserialize<Campaign>(campaignData);

    campaign!.AdCreative!.AdvertLinkAddress = $"/Uploads/{fileName}";
    campaign.Created = DateTime.Now;
    var result = db.Campaigns.Add(campaign);
    db.SaveChanges();

    return Results.Ok(result.Entity);
});


sellside.MapPost("/adcampaign", async (
    Campaign campaign, HttpContext context, 
    [FromServices] ApplicationDbContext dbContext, 
    [FromServices] RedisService redisCache) =>
{
    try
    {
        var formFile = context.Request.Form.Files["image"];

        if (campaign != null)
        {
            var creative = campaign.AdCreative;
            dbContext.AdCreatives.Add(creative!);
            var newCampaign = dbContext.Campaigns.Add(campaign);

            dbContext.SaveChanges();

            if (formFile != null && formFile.Length > 0)
            {
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                var cacheKey = newCampaign.Entity.Id.ToString();

                await redisCache.SaveFileAsync(
                    cacheKey, File.ReadAllBytes(filePath),
                    TimeSpan.FromDays(30));

                return Results.Ok(newCampaign);
            }
        }
        return Results.BadRequest();
    }
    catch(Exception ex)
    {
        return Results.BadRequest();
    }
});

sellside.MapGet("/adcampaign/{campaignId}", async (
    string campaignId,
    [FromServices] ApplicationDbContext dbContext, 
    [FromServices] RedisService redisCache) =>
{

    if(campaignId is not null)
    {

        var campaign = await redisCache.GetCampaignAsync<Campaign>(campaignId);

        if (campaign is not null)
            return Results.Ok(campaign);

        campaign = dbContext.Campaigns
            .Include(c => c.AdCreative)
            .Include(c => c.User)
            .FirstOrDefault(c => c.Id.Equals(int.Parse(campaignId)));

        await redisCache.SaveCampaignAsync<Campaign>(
            key: campaignId,
            value: campaign,
            expiry: TimeSpan.FromMinutes(30));

        return Results.Ok(campaign);
    }

    return Results.BadRequest();
});

sellside.MapGet("/adcampaigns/{uid}", async (
    int uid,
    [FromServices] ApplicationDbContext dbContext,
    [FromServices] RedisService redisCache) =>
{
    if (uid is not 0)
    {
        var campaigns = await redisCache.GetCampaignsAsync<List<Campaign>>(uid.ToString());

        if (campaigns is not null)
            return Results.Ok(campaigns);

        campaigns = dbContext.Campaigns
            .Where(campaign => campaign.UserId.Equals(uid))
            .Include(campaign => campaign.AdCreative)
            .Include(campaign => campaign.User)
            .ToList();

        await redisCache.SaveSitesAsync<List<Campaign>>(
            key: uid.ToString(),
            value: campaigns,
            expiry: TimeSpan.FromMinutes(30));

        return Results.Ok(campaigns);
    }

    return Results.BadRequest();
});

var demandside = app.MapGroup("demandside");

demandside.MapGet("/sites/{uid}", async (
    int uid,
    [FromServices] ApplicationDbContext dbContext,
    [FromServices] IHttpContextAccessor httpContextAccessor,
    [FromServices] RedisService redisCache) =>
{
    if(uid is not 0)
    {
        var sites = await redisCache.GetSitesAsync<List<RegisteredWebsite>>(uid.ToString());

        if (sites is not null)
            return Results.Ok(sites);

        var registeredWebsites = dbContext.RegisteredWebsites
            .Where(site => site.UserID.Equals(uid))
            .Include(s => s.User).ToList();

        registeredWebsites
            .ForEach(s => s.BaseUri = httpContextAccessor.HttpContext?.Request.Host.Value);

        await redisCache.SaveSitesAsync<List<RegisteredWebsite>>(
            key: uid.ToString(),
            value: registeredWebsites,
            expiry: TimeSpan.FromMinutes(30));

        return Results.Ok(registeredWebsites);
    }

    return Results.BadRequest();
});

demandside.MapGet("/site/{id}", (int id, 
    [FromServices] ApplicationDbContext dbContext) =>
{
    if(id is not 0)
    {
        var registeredSite = dbContext.RegisteredWebsites.FirstOrDefault(site => site.Id == id);

        if(registeredSite is null)
            return Results.NotFound();

        return Results.Ok(registeredSite);
    }

    return Results.BadRequest();
});

demandside.MapGet("/adsenable/{id}", (
    int id,
    [FromServices] ApplicationDbContext dbContext) =>
{
    if (id is not 0)
    {
        var site = dbContext.RegisteredWebsites.FirstOrDefault(rw => rw.Id == id);
        if (site is null)
            return Results.NotFound();

        site.AdsEnabled = true;

        dbContext.SaveChanges();
        return Results.Ok(site);
    }
    return Results.BadRequest();
});

demandside.MapGet("/adsdisable/{id}", (
    int id,
    [FromServices] ApplicationDbContext dbContext) =>
{
    if (id is not 0)
    {
        var site = dbContext.RegisteredWebsites.FirstOrDefault(rw => rw.Id == id);
        if (site is null)
            return Results.NotFound();

        site.AdsEnabled = false;

        dbContext.SaveChanges();
        return Results.Ok(site);
    }
    return Results.BadRequest();
});

demandside.MapPost("/sites", (
    RegisteredWebsite website,
    [FromServices] ApplicationDbContext dbContext) =>
{
    if(website is not null)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            website.CreatedDate = DateTime.Now;
            if(dbContext.RegisteredWebsites.Any(rw => rw.Domain.ToLower().Equals(website.Domain.ToLower())))
            {
                return Results.Forbid();
            }
            var entry = dbContext.RegisteredWebsites.Add(website);

            if (entry is null)
                return Results.BadRequest();

            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{website.UserID}:{entry.Entity.Id}:{entry.Entity.CreatedDate}"));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            entry.Entity.TagHashCode = builder.ToString();

            dbContext.SaveChanges();

            return Results.Ok(entry.Entity);
        }
    }

    return Results.BadRequest();
});

var bidding = app.MapGroup("bidding");

bidding.MapGet("getmatch/{fingerprint}", (
    string fingerprint,
    [FromServices] ApplicationDbContext dbContext,
    [FromServices] IHttpContextAccessor httpContextAccessor,
    [FromServices] RedisService redisCache) =>
{
    if(fingerprint is not null)
    {
        try
        {
            var site = dbContext.RegisteredWebsites.FirstOrDefault(w => w.TagHashCode == fingerprint);

            if (site is null)
            {
                return Results.Forbid();
            }

            var campaigns = dbContext.Campaigns
                .Where(c => (c.SafetyComponents!.Intersect(site!.Audience!).Any() | c.TargetSites!.Intersect(site.Categories!).Any()) && c.IsActive && c.DailyBudget > 0)
                .Include(c => c.AdCreative)
                .Select(c => c);

            if (campaigns is null)
            {
                var desperate = dbContext.Campaigns.FirstOrDefault(c => c.TargetSites!.Contains("Any"));
                if (desperate is not null)
                {
                    return Results.Ok(desperate);
                }
                return Results.NotFound();
            }

            if (campaigns.Count() > 1)
            {
                if (site.MonetizationType is PaymentType.ANY)
                {

                }
                else
                {
                    campaigns = campaigns.Where(c => c.PaymentType == site.MonetizationType);
                }
            }

            if (!campaigns.Any(c => c.Appearances.Where(s => s.SiteId == site.Id).Any()))
            {
                campaigns.ForEachAsync(c => c.Appearances.Add(new Randomizer { SiteId = site.Id, Appearances = 0 }));
            }

            if (!campaigns.All(c => c.Appearances.Any(a => a.SiteId == site.Id)))
            {
                campaigns.Where(c => c.Appearances.Any(a => a.SiteId == site.Id) == false).ForEachAsync(c => c.Appearances.Add(new Randomizer { SiteId = site.Id, Appearances = 0 }));
            }

            if (campaigns.Count() > 1)
            {
                campaigns = campaigns
                    .OrderByDescending(c => c.SafetyComponents!.Intersect(site!.Audience!).Count())
                    .ThenByDescending(c => c.TargetSites!.Intersect(site.Categories!).Count())
                    .ThenByDescending(c => c.MaxBiddingAmount)
                    .ThenBy(c => c.Appearances.FirstOrDefault(a => a.SiteId == site.Id)!.Appearances)
                    .ThenByDescending(c => c.Created);
            }

            var campaign = campaigns.First();
            campaign.Appearances.FirstOrDefault(a => a.SiteId == site.Id)!.Appearances += 1;

            string height = campaign.AdCreative.Height.ToString() + "px";
            string width = campaign.AdCreative.Width.ToString() + "px";

            var advert = new Ad(campaign.Id, $"https://{httpContextAccessor.HttpContext?.Request.Host.Value}" +
                $"{campaign.AdCreative.AdvertLinkAddress}", campaign.AdCreative.Heading,
                campaign.AdCreative.Description, campaign.AdvertLinkAddress, campaign.AdvertStyle,
                campaign.Orientation, height: height, width: width);

            return Results.Ok(advert);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError();
        }
    }
    return Results.Forbid();
});

var reporting = app.MapGroup("reporting");

reporting.MapPost("/metrics/{id}", (int id, EventData data, ApplicationDbContext db) =>
{
    if(id is not 0)
    {
        var report = db.Reports.FirstOrDefault(r => r.CampaignId == id);
        if(report is null)
        {
            Report newReport = new Report()
            {
                CampaignId = id,
                Clicks = new List<Click>(),
                Conversions = new List<Conversion>(),
                Wiews = new List<View>(),
                Hovers = new List<Hover>()
            };
            report = db.Reports.Add(newReport).Entity;
        }
        if(data.name.ToLower().Equals("click"))
        {
            report.Clicks.Add(new Click { Created = DateTime.Now });
        }
        if(data.name.ToLower().Equals("show"))
        {
            report.Wiews.Add(new View { Created = DateTime.Now });
        }
        if(data.name.ToLower().Equals("hover"))
        {
            report.Hovers.Add(new Hover { Created = DateTime.Now, Duration = data.count});
        }
    }
});

reporting.MapGet("/metrics/{id}", (int id, ApplicationDbContext db) =>
{
    if(id is not 0)
    {
        var report = db.Reports.FirstOrDefault(r => r.CampaignId == id);
        if (report is null)
            return Results.NotFound();

        return Results.Ok(report);
    }
    return Results.BadRequest();
});

app.Run();
record EventData(string name, int count, int id);
record Ad(int id, string? imagelink, string? heading, string? description, string? redirect, string? type, string? orientation, string? height, string? width);
