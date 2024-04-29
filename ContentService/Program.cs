using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ContentService.Components;
using ContentService.Components.Account;
using ContentService.Data;
using SharedModels.Services;
using SharedModels.Models;
using Radzen;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddRadzenComponents();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddHttpClient<IDataStore, DataStore>(config =>
{
    config.BaseAddress = new Uri(builder.Configuration["DataStore"]!);
});
builder.Services.AddSingleton<User>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:7113";
        options.ClientId = "publisherid";
        options.ClientSecret = "secret";

        options.UsePkce = true;
        options.SaveTokens = true;
    })
    .AddIdentityCookies();
Uri baseUri = new Uri(builder.Configuration["IdentityServer"]!);
builder.Services.Configure<IdentityServerSettings>(o =>
{
    o.DiscoveryUrl = "https://localhost:7113/.well-known/openid-configuration";
    o.UseHttps = true;
    o.ClientName = "publishername";
    o.ClientPassword = "secret";
});

builder.Services.AddSingleton<TokenService>();
builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("CloudDatabase") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode().RequireAuthorization();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
