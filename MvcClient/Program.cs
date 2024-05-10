using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


// grant_type is a flow of access token request(OAuth)
// respone_type is authorization flow type (OIDC)
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    config.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    config.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    // defined how to retrive access token
    .AddOpenIdConnect(config =>
    {
        config.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        config.Authority = "https://localhost:7299/"; // OIDC middleware will know how to retrieve discovery document
        config.ClientId = "client_id_mvc";
        config.ClientSecret = "client_secret_mvc";
        config.SaveTokens = true; // save token in our cookie

        config.ResponseType = "code";

        // configure cookie, claims mapping
        config.ClaimActions.DeleteClaim("amr");
        config.ClaimActions.DeleteClaim("s_hash");
        config.ClaimActions.MapUniqueJsonKey("RawCoding.Grandma","rc.garndma");

        // after get id_token it will do another round trip to get claims
        // to trip to load cliams in to the cookie but the size of id token will be smaller
        config.GetClaimsFromUserInfoEndpoint = true;
        config.Scope.Clear();
        config.Scope.Add("openid");
        config.Scope.Add("profile");
        config.Scope.Add("rc.scope");

        // both api will share the same access_token and claims
        config.Scope.Add("apione");
        config.Scope.Add("apitwo"); // if any claim is defined for this scope then apione will also have them.

        config.Scope.Add("offline_access"); // refresh token
    });

builder.Services.AddHttpClient();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
