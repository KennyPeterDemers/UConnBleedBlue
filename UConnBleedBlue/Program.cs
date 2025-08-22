using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Security.Claims;
using UConnBleedBlue.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => {
        options.DetailedErrors = true;
    });
builder.Services.AddScoped<PlayersService>();
builder.Services.AddScoped<CostsService>();
builder.Services.AddScoped<DonationsService>();

builder.Services.AddSingleton<UserValidationService>();

builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient
    {
        BaseAddress = new Uri(navigationManager.BaseUri)
    };
});

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/login";
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // ✅ Add this before UseAuthorization
app.UseAuthorization();

// ✅ Place minimal API login here
app.MapPost("/auth/login", async (HttpContext context, HttpRequest request, UserValidationService userValidator) =>
{
    var form = await request.ReadFormAsync();
    var username = form["username"].ToString();

    if (string.IsNullOrWhiteSpace(username) || !userValidator.IsValidUser(username))
        return Results.BadRequest("Invalid user");

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
    var identity = new ClaimsIdentity(claims, "MyCookieAuth");
    var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync("MyCookieAuth", principal);
    return Results.Ok();
});

app.MapGet("/login-redirect", (HttpContext context) =>
{
    return Results.Redirect("/index");
});

app.MapControllers(); // Add this before MapBlazorHub
app.MapRazorPages();          // ⬅️ add this
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        Console.WriteLine("🔥 Unhandled middleware exception:");
        Console.WriteLine(ex.ToString());
        throw;
    }
});

app.Run();
