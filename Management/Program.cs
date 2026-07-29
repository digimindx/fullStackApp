using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // <-- تأكد من وجود هذا الـ Namespace
using Core.Data;
using Core.Interfaces;
using Core.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Fetch the Connection String from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Register AppDbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Register the Repository Dependency Injection (Scoped lifetime)
builder.Services.AddScoped<IEmployee, EmployeeRepository>();

// =========================================================================
// هام جداً: تسجيل خدمات الـ Cookie Authentication ليتعرف عليها الـ Controller
// =========================================================================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";   // المسار البديل في حال لم يكن مسجلاً للدخول
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromSeconds(20);
    });

// Add services to the container (MVC standard services)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// =========================================================================
// هام جداً: تفعيل الـ Authentication Middleware قبل الـ Authorization والـ Routing
// =========================================================================
app.UseAuthentication(); // <-- تفعيل التحقق من الهوية (قراءة وكتابة الـ Cookies)
app.UseAuthorization();

// Standard MVC Routing structure
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();