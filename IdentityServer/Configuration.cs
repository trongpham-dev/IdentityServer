using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Configuration
    {
        // in addtion to api scope defined before we also need to define scope for user can get in4 claims (user name, email,...)
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource> { 
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),

                };
        // define which resource need to protect
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> { new ApiResource("ApiOne"), new ApiResource("ApiTwo") };

        //define client that can consume the resource - request the token

        public static IEnumerable<Client> GetClients() =>
            new List<Client> { 
                new Client { 
                    ClientId = "client_id", 
                    ClientSecrets = {
                        new Secret("client_secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"ApiOne"}
                },
                new Client {
                    ClientId = "client_id_mvc",
                    ClientSecrets = {
                        new Secret("client_secret_mvc".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {"ApiOne", "ApiTwo", "openid", "profile"}, // try to use constant instead. IdentityServerConstants.StandardScopes.Profile
                    RedirectUris = { "https://localhost:7147/signin-oidc" }
                }
            };
    }
}
