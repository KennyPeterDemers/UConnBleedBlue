using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Security.Claims;
using UConnBleedBlue.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====  LOG PATH (robust)  =====
var preferredLogsDir = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "logs");
string logsDir;
try
{
    Directory.CreateDirectory(preferredLogsDir);
    logsDir = preferredLogsDir;
}
catch
{
    var temp = Path.Combine(Path.GetTempPath(), "uconnbleedblue", "logs");
    Directory.CreateDirectory(temp);
    logsDir = temp;
}

// =====  SERVICES  =====
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(o => o.DetailedErrors = true);
builder.Services.AddScoped<PlayersService>();
builder.Services.AddScoped<CostsService>();
builder.Services.AddSingleton<DonationsService>();
builder.Services.AddSingleton<UserValidationService>();

builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigationManager.BaseUri) };
});

builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", o => o.LoginPath = "/login");
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

// =====  SERILOG (defensive)  =====
try
{
    // Serilog internal troubles -> write here (never throws)
    Serilog.Debugging.SelfLog.Enable(msg =>
    {
        try
        {
            var selfLogPath = Path.Combine(Path.GetTempPath(), "uconnbleedblue", "serilog-selflog.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(selfLogPath)!);
            File.AppendAllText(selfLogPath, $"{DateTime.UtcNow:O} {msg}{Environment.NewLine}");
        }
        catch { /* swallow */ }
    });

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Async(a => a.File(
            path: Path.Combine(logsDir, "uconn-.log"),
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 10,
            fileSizeLimitBytes: 10_000_000,
            rollOnFileSizeLimit: true,
            shared: true,               // OK on IIS
            encoding: Encoding.UTF8))   // no 'buffered' param
        .CreateLogger();

    Log.Information("App starting in {Environment}; logsDir={LogsDir}", builder.Environment.EnvironmentName, logsDir);
    builder.Host.UseSerilog();
}
catch (Exception ex)
{
    // If Serilog fails, DO NOT crash the app.
    // Fall back to default console logging so the site still runs.
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    Console.WriteLine($"[Startup] Serilog init failed: {ex}");
}

var app = builder.Build();

// PRELOAD donations once at startup (before the app starts serving requests)
using (var scope = app.Services.CreateScope())
{
    var donations = scope.ServiceProvider.GetRequiredService<DonationsService>();
    await donations.EnsureLoadedAsync();  // calls your method

    // Optional: log a quick summary to verify it ran
    if (!string.IsNullOrWhiteSpace(donations.Error))
    {
        Log.Warning("Donations preload completed with Error: {Error}", donations.Error);
    }
    else
    {
        Log.Information("Donations preload OK. Rows: {Count}, Total: {Total}",
            donations.DonationList.Count, donations.TotalDonations);
    }
}
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

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}

