using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using OnlineLibrary.Components;
using OnlineLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<CustomUser, CustomRole>(
options => {
    options.Stores.MaxLengthForKeys = 128;
    // options.Password.RequiredLength = 8;
    // options.Password.RequireNonAlphanumeric = true;
    // options.Password.RequireUppercase = true;
    // options.Password.RequireLowercase = true;
    // options.Password.RequireDigit = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddRoles<CustomRole>()
.AddDefaultUI()
.AddDefaultTokenProviders();


builder.Services.AddControllersWithViews();

builder.Services
.AddRazorComponents()
.AddInteractiveServerComponents()
.AddCircuitOptions(options => options.DetailedErrors = true); // for debugging razor components

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BookService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAntiforgery();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapRazorComponents<App>()
.AddInteractiveServerRenderMode();

app.Run();
