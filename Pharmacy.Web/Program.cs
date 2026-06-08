using Microsoft.EntityFrameworkCore;
using Pharmacy.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region DatabaseConfig
var connectionString = builder.Configuration.GetConnectionString("Pharmacy_Project");

builder.Services.AddDbContext<PharmacyDbContext>(option =>
option.UseSqlServer(connectionString), ServiceLifetime.Transient);
#endregion


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
