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
            new ApiScope(name: "weatherapi.read", 
                new List<string>{
                    "role",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile
                }),
            new ApiScope(name: "weatherapi.write", new List<string>{"role", "email" }),
            new ApiScope(name: "publisher.read", new List<string>{"role", "email" }),
            new ApiScope(name: "publisher.write", new List<string>{"role", "email" }),
            new ApiScope(name: "advertiser.read", new List<string>{"role", "email" }),
            new ApiScope(name: "advertiser.write", new List<string>{"role", "email" }),
            new ApiScope(name: "testpublisher.read", new List<string>{"role", "email" }),
            new ApiScope(name: "testpublisher.write", new List<string>{"role", "email" }),
        };

    public static IEnumerable<ApiResource> ApiResourses => new[]
    {
        new ApiResource("weatherapi")
        {
            Scopes = new[] { "weatherapi.read", "weatherapi.write" },
            ApiSecrets = new List<Secret> {new Secret(value: "ScopeSecret".Sha256())},
            UserClaims = new List<string>{"role", "email" }
        },
        new ApiResource("AdvertisersWebService")
        {
            Scopes = new[] { "advertiser.read", "advertiser.write" },
            ApiSecrets = new List<Secret> {new Secret(value: "ScopeSecret".Sha256())},
            UserClaims = new List<string>{"role","email" }
        },
        new ApiResource("ContentService")
        {
            Scopes = new[] { "publisher.read", "publisher.write" },
            ApiSecrets = new List<Secret> {new Secret(value: "ScopeSecret".Sha256())},
            UserClaims = new List<string>{"role", "email" }
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
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                RequireConsent = false,
                // scopes that client has access to
                AllowedScopes = { "weatherapi.read", "weatherapi.write" }
            },
            new Client
            {
                ClientId = "testpublisherid",
                ClientName = "testpublishername",

                RedirectUris = {"https://localhost:7600/signin-oidc"},
                FrontChannelLogoutUri = "https://localhost:7600/signout-oidc",
                PostLogoutRedirectUris = {"https://localhost:7600/signout-callback-oidc"},
                // no interactive user, use the clientid/secret for authentication

                AllowedGrantTypes = new[] {
                    GrantType.Implicit,
                    GrantType.ResourceOwnerPassword,
                    GrantType.ClientCredentials
                },

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                RequireConsent = false,
                // scopes that client has access to

                AllowOfflineAccess = true,
                RequirePkce = true,
                AllowPlainTextPkce = false,
                AllowedScopes = {
                    "testpublisher.read", "testpublisher.write",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile
                }
            },
            new Client
            {
                ClientId = "advertiserid",
                ClientName = "advertisername",

                // no interactive user, use the clientid/secret for authentication
                
                AllowedGrantTypes = new[] {
                    GrantType.Implicit,
                    GrantType.ResourceOwnerPassword,
                    GrantType.ClientCredentials
                },

                RedirectUris = {"https://advertiserswebservice/signin-oidc"},
                FrontChannelLogoutUri = "https://advertiserswebservice/signout-oidc",
                PostLogoutRedirectUris = {"https://advertiserswebservice/signout-callback-oidc"},

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                RequireConsent = false,
                AllowOfflineAccess = true,
                RequirePkce = true,
                AllowPlainTextPkce = false,
                // scopes that client has access to
                AllowedScopes = {
                    "advertiser.read", "advertiser.write",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile
                }
            },
            new Client
            {
                ClientId = "publisherid",
                ClientName = "publishername",

                // no interactive user, use the clientid/secret for authentication
                
                AllowedGrantTypes = new[] {
                    GrantType.Implicit,
                    GrantType.ResourceOwnerPassword,
                    GrantType.ClientCredentials
                },

                RedirectUris = {"https://contentservice/signin-oidc"},
                FrontChannelLogoutUri = "https://contentservice/signout-oidc",
                PostLogoutRedirectUris = {"https://contentservice/signout-callback-oidc"},

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                AllowOfflineAccess = true,
                RequirePkce = true,
                AllowPlainTextPkce = false,
                RequireConsent = false,
                // scopes that client has access to
                AllowedScopes = {
                    "publisher.read", "publisher.write",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile
                }
            },

            // interactive client using code flow + pkce
            new Client
            {
              ClientId = "interactive",
              ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},

              AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

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