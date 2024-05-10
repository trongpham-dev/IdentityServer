
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Configuration
    {
        // in addtion to api scope defined before we also need to define scope for user can get in4 claims (user name, email,...)
        // added to id_token
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource> { 
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    //define custom scope for the claims that will be attached to id_token
                    new IdentityResource
                    {
                        Name = "rc.scope",
                        UserClaims =
                        {
                            "rc.garndma"
                        }
                    }

                };
        // define which resource need to protect with respective scope
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> { 
                new ApiResource {
                    Name = "apione", Enabled = true,
                    Scopes = new List<string> { "apione"},
                    UserClaims = new List<string> { "rc.api.garndma"} 
                },
                new ApiResource {
                    Name = "apitwo", Enabled = true,
                    Scopes = new List<string> { "apitwo"},
                    UserClaims = new List<string> { "rc.api.garndma"}
                } };

        // Add to your ApiScopes (not ApiResources)
        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
                new ApiScope("apione"),
                new ApiScope("apitwo"),
           };

        //define client that can consume the resource - request the token

        public static IEnumerable<Client> GetClients() =>
            new List<Client> { 
                new Client { 
                    ClientId = "client_id", 
                    ClientSecrets = {
                        new Secret("client_secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials, // machine to machine comunication does not need user password(just client_id, secret)
                    AllowedScopes = { "apione" }
                },
                new Client {
                    ClientId = "client_id_mvc",
                    ClientSecrets = {
                        new Secret("client_secret_mvc".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = { "apione", "apitwo", "openid", "profile", "rc.scope"}, // try to use constant instead. IdentityServerConstants.StandardScopes.Profile
                    RedirectUris = { "https://localhost:7147/signin-oidc" },

                    // puts all the claims in the id token
                    //AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true, // enable refresh_token because it's optional by default
                    RequireConsent = false
                }
            };
    }
}
