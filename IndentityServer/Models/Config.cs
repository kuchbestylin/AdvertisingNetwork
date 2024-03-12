using System.Collections.Generic;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IndentityServer.Models;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            },
            new IdentityResource("role", new[] {"role"}),
            new IdentityResource("color", new [] { "favorite_color" })

        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(name: "weatherapi.read"),
            new ApiScope(name: "weatherapi.write")
        };

    public static IEnumerable<ApiResource> ApiResourses => new[]
    {
        new ApiResource("weatherapi")
        {
            Scopes = new[] { "weatherapi.read", "weatherapi.write" },
            ApiSecrets = new List<Secret> {new Secret(value: "ScopeSecret".Sha256())},
            UserClaims = new List<string>{"role"}
        }
    };
    
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "client",
                ClientName = "client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                RequireConsent = false,
                // scopes that client has access to
                AllowedScopes = { "weatherapi.read", "weatherapi.write" }
            },
            // interactive ASP.NET Core Web App
            new Client
            {
                ClientId = "web",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
                
                // where to redirect to after login
                RedirectUris = { "https://localhost:5002/signin-oidc" },

                // where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                AllowOfflineAccess = true,

                RequireConsent = false,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "weatherapi.read",
                    "weatherapi.write"
                }
            },

            // interactive client using code flow + pkce
            new Client
            {
              ClientId = "interactive",
              ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},

              AllowedGrantTypes = GrantTypes.Code,

              RedirectUris = {"https://localhost:7002/signin-oidc"},
              FrontChannelLogoutUri = "https://localhost:7002/signout-oidc",
              PostLogoutRedirectUris = {"https://localhost:7002/signout-callback-oidc"},

              AllowOfflineAccess = true,
              AllowedScopes = {"openid", "profile", "weatherapi.read"},
              RequirePkce = true,
              RequireConsent = false,
              AllowPlainTextPkce = false
            }
        };
}