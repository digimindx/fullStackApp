using Core.Data;
using Core.Interfaces;
using Core.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Allow Cross-Origin requests from the React dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins("http://localhost:59268")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 1. Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Repository Dependency Registration
builder.Services.AddScoped<IEmployee, EmployeeRepository>();

// =========================================================================
// 3. CORRECT WAY: Register Cookie Authentication ONLY for MVC
// =========================================================================
// تسجيل خدمات الـ Cookie Authentication مع تحديد الـ Default Schemes صراحةً
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

// 4. Register MVC Views and Controllers
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable CORS before authentication/authorization so preflight is handled
app.UseCors("AllowClient");

// Middleware order matters! Auth must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

// Standard MVC View Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();