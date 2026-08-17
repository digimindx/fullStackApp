// ─── استدعاء المكتبات المطلوبة (يجب أن تكون في الأعلى دائماً) ─────────
using CORE.Data;                // للوصول إلى AppDbContext
using CORE.Interfaces.HR;       // للوصول إلى واجهات المستودعات
using CORE.Repositories.HR;     // للوصول إلى تنفيذات المستودعات
using Microsoft.EntityFrameworkCore; // للوصول إلى UseSqlServer

var builder = WebApplication.CreateBuilder(args);

// ─── إضافة الخدمات إلى الحاوية ──────────────────────────────────────
// تسجيل خدمات MVC لدعم Controllers و Views
builder.Services.AddControllersWithViews();

// تسجيل DbContext للاتصال بقاعدة البيانات
// تأكد من وجود سلسلة الاتصال "DefaultConnection" في appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// تسجيل مستودع الموظفين كخدمة Scoped (نسخة جديدة لكل طلب HTTP)
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// تسجيل مستودع عناوين الموظفين كخدمة Scoped
builder.Services.AddScoped<IEmployeeAddressRepository, EmployeeAddressRepository>();

// تسجيل مستودع حسابات الموظفين كخدمة Scoped
builder.Services.AddScoped<IEmployeeAccountRepository, EmployeeAccountRepository>();

// تسجيل مستودع الحسابات البنكية للموظفين كخدمة Scoped
builder.Services.AddScoped<IEmployeeBankAccountRepository, EmployeeBankAccountRepository>();

// ─── بناء التطبيق ───────────────────────────────────────────────────
var app = builder.Build();

// ─── إعداد خط معالجة طلبات HTTP ──────────────────────────────────────
// إذا لم تكن البيئة تطويرية، تفعيل صفحة معالجة الأخطاء و HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// إعادة توجيه HTTP إلى HTTPS تلقائياً
app.UseHttpsRedirection();

// تفعيل التوجيه (Routing) لتحديد(Controller) و(Action) المناسب
app.UseRouting();

// تفعيل المصادقة (التحقق من هوية المستخدم)
app.UseAuthentication();

// تفعيل الصلاحيات (التحقق من صلاحية الوصول)
app.UseAuthorization();

// تعيين الملفات الثابتة (CSS, JS, Images)
app.MapStaticAssets();

// تعريف المسار الافتراضي: Home/Index هو الصفحة الرئيسية
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// ─── تشغيل التطبيق ──────────────────────────────────────────────────
app.Run();