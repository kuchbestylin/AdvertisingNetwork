using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SharedModels.Models;
using SharedModels.Services;
using Test.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "oidc";

}).AddCookie("cookie")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:7113";
    options.ClientId = "interactive";
    options.ClientSecret = "SuperSecretPassword";

    options.ResponseType = "code";
    options.UsePkce = true;
    options.ResponseMode = "query";

    options.Scope.Add("weatherapi.read");
    options.Scope.Add("openid");
    options.Scope.Add("offline_access");
    options.SaveTokens = true;
});
builder.Services.AddHttpClient();
builder.Services.Configure<IdentityServerSettings>(o =>
{
    o.DiscoveryUrl = "https://localhost:7113/.well-known/openid-configuration";
    o.UseHttps = true;
    o.ClientName = "client";
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
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode().RequireAuthorization();

app.Run();
