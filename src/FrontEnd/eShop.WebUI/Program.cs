using eShop.WebUI.Services.Identity;
using eShop.WebUI.Services.ProductApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Products/Index", "");
});
builder.Services.AddAuthentication("X-WebUI-Cookie")
    .AddCookie("X-WebUI-Cookie");

builder.Services.AddHttpClient<IIdentityApiClient, IdentityApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:IdentityApi"]);
});

builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:ProductApi"]);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
