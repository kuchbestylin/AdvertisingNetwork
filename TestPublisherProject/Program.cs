using SharedModels.Models;
using SharedModels.Services;
using TestPublisherProject.Components;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddCascadingAuthenticationState();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "oidc";

}).AddCookie("cookie")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:7113";
    options.ClientId = "testpublisherid";
    options.ClientSecret = "secret";

    options.TokenValidationParameters = 
        new TokenValidationParameters() { NameClaimType = "email" };

    options.UsePkce = true;
    options.SaveTokens = true;
});
builder.Services.Configure<IdentityServerSettings>(o =>
{
    o.DiscoveryUrl = "https://localhost:7113/.well-known/openid-configuration";
    o.UseHttps = true;
    o.ClientName = "testpublishername";
    o.ClientPassword = "secret";
});

builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
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
    .AddInteractiveServerRenderMode().RequireAuthorization();

app.Run();
