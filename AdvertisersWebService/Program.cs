using AdvertisersWebService.Components;
using Microsoft.AspNetCore.Identity;
using Radzen;
using SharedModels.Services;
using SharedModels.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpClient<IDataStore, DataStore>(config =>
{
    config.BaseAddress = new Uri(builder.Configuration["DataStore"]!);
});
builder.Services.AddSingleton<User>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
})
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:7113";

        options.ClientId = "advertiserid";
        options.ClientSecret = "secret";

        options.UsePkce = true;
        options.SaveTokens = true;
    });

builder.Services.Configure<IdentityServerSettings>(o =>
{
    o.DiscoveryUrl = $"{builder.Configuration["IdentityServer"]}/.well-known/openid-configuration";

    o.UseHttps = true;
    o.ClientName = "advertisername";
    o.ClientPassword = "secret";
});

builder.Services.AddAuthorization();
builder.Services.AddSingleton<TokenService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();

app.Run();
