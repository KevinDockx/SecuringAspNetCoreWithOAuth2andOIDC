using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Marvin.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
           [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new("roles",
                "Your role(s)",
                ["role"]),
            new("country",
                "The country you're living in",
                ["country"])
           ];

    public static IEnumerable<ApiResource> ApiResources =>
     [
             new("imagegalleryapi",
                 "Image Gallery API",
                 ["role", "country"])
             {
                 Scopes = { "imagegalleryapi.fullaccess",
                     "imagegalleryapi.read",
                     "imagegalleryapi.write"},
                ApiSecrets = { new Secret("apisecret".Sha256()) }
             }
         ];
     
    public static IEnumerable<ApiScope> ApiScopes =>
        [
                new("imagegalleryapi.fullaccess"),
                new("imagegalleryapi.read"),
                new("imagegalleryapi.write")];


    public static IEnumerable<Client> Clients =>
        [
                new()
                {
                    ClientName = "Image Gallery",
                    ClientId = "imagegalleryclient",
                    AllowedGrantTypes = GrantTypes.Code,
                    AccessTokenType = AccessTokenType.Reference,
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AccessTokenLifetime = 120,
                    // AuthorizationCodeLifetime = ...
                    // IdentityTokenLifetime = ...
                    RedirectUris =
                    {
                        "https://localhost:7184/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:7184/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        //"imagegalleryapi.fullaccess",
                        "imagegalleryapi.read",
                        "imagegalleryapi.write",
                        "country"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = true
                },
                new()
                {
                    ClientName = "Image Gallery BFF",
                    ClientId = "imagegallerybff",                    
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,                    
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RedirectUris =
                    {
                        "https://localhost:7119/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:7119/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles", 
                        "imagegalleryapi.read",
                        "imagegalleryapi.write",
                        "country"
                    },
                    ClientSecrets =
                    {
                        new Secret("anothersecret".Sha256())
                    },
                    RequireConsent = true
                }
            ];
}