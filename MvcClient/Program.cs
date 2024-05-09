var builder = WebApplication.CreateBuilder(args);


// grant_type is a flow of access token request(OAuth)
// respone_type is authorization flow type (OIDC)
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = "Cookie";
    config.DefaultChallengeScheme = "oidc";
})
    .AddCookie("cookie")
    // defined how to retrive access token
    .AddOpenIdConnect("oidc", config =>
    {
        config.Authority = "https://localhost:7299/"; // OIDC middleware will know how to retrieve discovery document
        config.ClientId = "client_id_mvc";
        config.ClientSecret = "client_secret_mvc";
        config.SaveTokens = true; // save token in our cookie

        config.ResponseType = "code";
        config.Scope.Add("openid");
        config.Scope.Add("profile");
    });
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
