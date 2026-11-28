using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using _31241026329_TranHuuTuanBang.Data;
using _31241026329_TranHuuTuanBang.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "connection.txt");
if (string.IsNullOrWhiteSpace(connectionString) && File.Exists(connectionPath))
    connectionString = File.ReadAllText(connectionPath).Trim();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:RequireConfirmedAccount");
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
    options.AddPolicy("Checkout", context => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress?.ToString() ?? "unknown", _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 12,
            Window = TimeSpan.FromMinutes(10)
        }));
});

var posterDirectory = builder.Configuration["Storage:PosterDirectory"]
    ?? Path.Combine(builder.Environment.ContentRootPath, "App_Data", "posters");
Directory.CreateDirectory(posterDirectory);
builder.Services.AddSingleton(provider => new PosterStorage(posterDirectory, provider.GetRequiredService<ILogger<PosterStorage>>()));
builder.Services.AddScoped<EmailService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var priceCulture = CultureInfo.GetCultureInfo("en-US");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new(priceCulture, CultureInfo.GetCultureInfo("vi-VN")),
    SupportedCultures = [priceCulture],
    SupportedUICultures = [CultureInfo.GetCultureInfo("vi-VN")]
});
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(posterDirectory),
    RequestPath = "/posters",
    OnPrepareResponse = response => response.Context.Response.Headers["X-Content-Type-Options"] = "nosniff"
});
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapStaticAssets();
app.MapControllerRoute("default", "{controller=Movies}/{action=Index}/{id?}").WithStaticAssets();
app.MapRazorPages().WithStaticAssets();

await AdminSetup.ConfigureAsync(app.Services, app.Configuration);
app.Run();
