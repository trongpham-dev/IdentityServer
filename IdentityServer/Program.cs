using IdentityModel;
using IdentityServer;
using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(config =>
{
    config.UseInMemoryDatabase("Memory");
});


builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 4;
    config.Password.RequireDigit = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookie";
    config.LoginPath = "/Auth/Login";
});


//Identity server is just a toolbox allows us to set up the infrastructure for authen vs author using OIDC flows 
builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddInMemoryApiResources(Configuration.GetApis())
    .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
    .AddInMemoryClients(Configuration.GetClients())
    .AddInMemoryApiScopes(Configuration.ApiScopes)
    .AddDeveloperSigningCredential(); // generate developer certification to sign tokens (use secret key)

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seeding user
using(var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var user = new IdentityUser("bob");
    userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
    userManager.AddClaimAsync(user, new Claim("rc.garndma", "big.cookie")).GetAwaiter().GetResult(); // added to id_token
    userManager.AddClaimAsync(user, new Claim("rc.api.garndma", "big.api.cookie")).GetAwaiter().GetResult(); // added to access_token
}

app.UseRouting();

app.UseIdentityServer();

app.MapDefaultControllerRoute();

app.Run();
