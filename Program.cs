using Microsoft.EntityFrameworkCore;
using RetroGameStore.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


// Use /tmp on Railway (Linux) so SQLite has a writable path
var dbPath = OperatingSystem.IsLinux()
    ? "/tmp/RetroGameStore.db"
    : "RetroGameStore.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// Show detailed errors so we can debug on Railway
app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=LoginCustomer}/{id?}");

app.Run();
