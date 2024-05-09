using IdentityModel;
using IdentityServer;
using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    .AddInMemoryApiResources(Configuration.GetApis())
    .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
    .AddInMemoryClients(Configuration.GetClients())
    .AddDeveloperSigningCredential(); // generate developer certification to sign tokens (use secret key)

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seeding user
using(var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var user = new IdentityUser("bob");
    userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
}

app.UseRouting();

app.UseIdentityServer();

app.MapDefaultControllerRoute();

app.Run();
