using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CraftShack.Models;
using CraftShack.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CraftShackDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CraftShackDb")));

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // This enables Identity UI

var app = builder.Build(); // <-- Build the app first

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // This enables Identity UI

app.Run();
