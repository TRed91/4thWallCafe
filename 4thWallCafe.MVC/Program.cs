using _4thWallCafe.App;
using _4thWallCafe.MVC;
using _4thWallCafe.MVC.db;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Setup Identity for Authentication
builder.Services.AddDbContext<ApplicationDbContext>(opts => 
    opts.UseSqlServer(builder.Configuration["ConnectionString"]));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        //Password settings
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        
        // User settings
        options.User.RequireUniqueEmail = true;
        
        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Supply Services for injection
var config = new AppConfiguration(builder.Configuration);
var sf = new ServiceFactory(config);
builder.Services.AddScoped(_ => sf.GenerateItemService());
builder.Services.AddScoped(_ => sf.GenerateCafeOrderService());
builder.Services.AddScoped(_ => sf.GenerateServerService());

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default", 
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();