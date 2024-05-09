using IdentityServer;

var builder = WebApplication.CreateBuilder(args);

//Identity server is just a toolbox allows us to set up the infrastructure for authen vs author using OIDC flows 
builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(Configuration.GetApis())
    .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
    .AddInMemoryClients(Configuration.GetClients())
    .AddDeveloperSigningCredential(); // generate developer certification to sign tokens (use secret key)

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();

app.UseIdentityServer();

app.MapDefaultControllerRoute();

app.Run();
