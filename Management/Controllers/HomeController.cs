// استدعاء مكتبة التشفير لتوليد Hash و Salt بشكل آمن
using System.Security.Cryptography;
// استدعاء مكتبة الترميز لتحويل النصوص إلى بايتات
using System.Text;
// استدعاء مكتبة ASP.NET Core MVC للتحكم والعروض
using Microsoft.AspNetCore.Mvc;
// استدعاء نماذج العرض من مشروع CORE
using CORE.Models.HR;
// استدعاء الكيانات من مشروع CORE
using CORE.Entities.HR;
// استدعاء واجهات المستودعات من مشروع CORE
using CORE.Interfaces.HR;
// استدعاء EF Core للتعامل مع الاستثناءات
using Microsoft.EntityFrameworkCore;

namespace Management.Controllers
{
    /// <summary>
    /// المتحكم الرئيسي لإدارة عمليات التسجيل والصفحة الرئيسية
    /// </summary>
    public class HomeController : Controller
    {
        // متغير خاص لمستودع الموظفين
        private readonly IEmployeeRepository _employeeRepository;
        // متغير خاص لمستودع عناوين الموظفين
        private readonly IEmployeeAddressRepository _employeeAddressRepository;
        // متغير خاص لمستودع حسابات الموظفين
        private readonly IEmployeeAccountRepository _employeeAccountRepository;

        /// <summary>
        /// دالة البناء لحقن التبعيات (Dependency Injection)
        /// يتم تمرير واجهات المستودعات تلقائياً من حاوية الخدمات
        /// </summary>
        public HomeController(
            IEmployeeRepository employeeRepository,
            IEmployeeAddressRepository employeeAddressRepository,
            IEmployeeAccountRepository employeeAccountRepository)
        {
            // تعيين المستودعات المحقونة إلى المتغيرات الخاصة لاستخدامها لاحقاً
            _employeeRepository = employeeRepository;
            _employeeAddressRepository = employeeAddressRepository;
            _employeeAccountRepository = employeeAccountRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// عرض نموذج التسجيل (GET)
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            // إرجاع عرض فارغ لنموذج التسجيل
            return View();
        }

        /// <summary>
        /// معالجة تسجيل موظف جديد (POST)
        /// تستقبل بيانات النموذج وتقوم بإنشاء الموظف والعنوان والحساب المرتبط به
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] // حماية ضد هجمات تزوير الطلبات عبر المواقع (CSRF)
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // التحقق من صحة البيانات المدخلة حسب سمات التحقق المعرفة في RegisterViewModel
            if (!ModelState.IsValid)
            {
                // إذا كان النموذج غير صالح، إرجاع نفس العرض مع أخطاء التحقق للمستخدم
                return View(model);
            }

            try
            {
                // ─── 0. التحقق من عدم تكرار اسم المستخدم أو البريد الإلكتروني ──
                var existingByUsername = await _employeeAccountRepository.GetByUsernameAsync(model.Username);
                if (existingByUsername != null)
                {
                    ModelState.AddModelError("Username", "اسم المستخدم مستخدم بالفعل. يرجى اختيار اسم آخر.");
                    return View(model);
                }

                var existingByEmail = await _employeeAccountRepository.GetByEmailAsync(model.Email);
                if (existingByEmail != null)
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني مسجل بالفعل. يرجى استخدام بريد آخر.");
                    return View(model);
                }

                // ─── 1. إنشاء وحفظ كيان الموظف الأساسي ─────────────────────────
                var employee = new Employee
                {
                    EmployeeNumber = GenerateEmployeeNumber(), // ✅ توليد رقم موظف فريد
                    FullName = model.FullName,                 // تعيين الاسم الكامل من النموذج
                    LastName = model.LastName,                  // تعيين اسم العائلة من النموذج
                    Gender = model.Gender,                      // تعيين الجنس من النموذج
                    DateOfBirth = model.DateOfBirth,            // تعيين تاريخ الميلاد من النموذج
                    IsBornAbroad = false,                       // ✅ تعيين الحقل الإلزامي
                    HasDoubleNationality = false,               // ✅ تعيين الحقل الإلزامي
                    CreatedBy = "System",                       // ✅ تعيين حقل التدقيق
                    IsActive = true                             // ✅ تفعيل الموظف مباشرة
                };

                // إضافة الموظف إلى قاعدة البيانات
                await _employeeRepository.AddAsync(employee);

                // ─── 2. إنشاء وحفظ عنوان الموظف ────────────────────────────────
                var address = new EmployeeAddress
                {
                    EmployeeID = employee.EmployeeID,           // ✅ استخدام المعرف المولد من الخطوة السابقة
                    AddressType = 'C',                          // ✅ تعيين نوع العنوان (حالي)
                    IsPrimary = true,                           // ✅ تعيين كعنوان رئيسي
                    Email = model.Email,                        // تعيين البريد الإلكتروني من النموذج
                    CreatedBy = "System",                       // ✅ تعيين حقل التدقيق
                };

                // إضافة العنوان إلى قاعدة البيانات
                await _employeeAddressRepository.AddAsync(address);

                // ─── 3. توليد الملح (Salt) بشكل عشوائي وآمن تشفيرياً ───────────
                byte[] saltBytes = new byte[16]; // إنشاء مصفوفة بحجم 16 بايت (128 بت)
                using (var rng = RandomNumberGenerator.Create())
                {
                    // ملء المصفوفة ببايتات عشوائية آمنة تشفيرياً
                    rng.GetBytes(saltBytes);
                }

                // ─── 4. توليد الهاش (Hash) من كلمة المرور باستخدام PBKDF2 ─────
                // تحويل كلمة المرور النصية إلى مصفوفة بايت باستخدام ترميز UTF8
                byte[] passwordBytes = Encoding.UTF8.GetBytes(model.Password);

                // استخدام خوارزمية PBKDF2 مع SHA256 و 100,000 تكرار للأمان العالي
                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    passwordBytes, saltBytes, 100000, HashAlgorithmName.SHA256))
                {
                    // استخراج الهاش بحجم 32 بايت (256 بت)
                    byte[] hashBytes = pbkdf2.GetBytes(32);

                    // تحويل البايتات إلى نصوص Base64 لتخزينها في قاعدة البيانات
                    string passwordSalt = Convert.ToBase64String(saltBytes);
                    string passwordHash = Convert.ToBase64String(hashBytes);

                    // ─── 5. إنشاء وحفظ حساب الموظف ─────────────────────────────
                    var account = new EmployeeAccount
                    {
                        EmployeeID = employee.EmployeeID,       // ✅ استخدام المعرف المولد من الخطوة الأولى
                        Gender = model.Gender,                  // ✅ تعيين الجنس من النموذج
                        Username = model.Username,               // تعيين اسم المستخدم من النموذج
                        Email = model.Email,                     // تعيين البريد الإلكتروني من النموذج
                        PasswordHash = passwordHash,             // استخدام الهاش المولد محلياً
                        PasswordSalt = passwordSalt,             // استخدام الملح المولد محلياً
                        IsLocked = false,                        // ✅ تعيين كغير مقفل
                        FailedLoginAttempts = 0,                 // ✅ تصفير محاولات الفشل
                        CreatedBy = "System",                    // ✅ تعيين حقل التدقيق
                    };

                    // إضافة الحساب إلى قاعدة البيانات
                    await _employeeAccountRepository.AddAsync(account);
                }

                // ─── 6. إعادة التوجيه إلى صفحة الفهرس بعد النجاح ──────────────
                return RedirectToAction("Index", "HR");
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("unique") == true || 
                                                ex.InnerException?.Message.Contains("duplicate") == true ||
                                                ex.InnerException?.Message.Contains("constraint") == true)
            {
                // معالجة أخطاء المفاتيح المكررة بشكل محدد
                ModelState.AddModelError("", "حدث خطأ: البيانات مسجلة بالفعل. يرجى التحقق من المعلومات المدخلة.");
                return View(model);
            }
            catch (Exception ex)
            {
                // إضافة رسالة خطأ عامة للنموذج دون تسريب تفاصيل الاستثناء لأسباب أمنية
                ModelState.AddModelError("", "حدث خطأ أثناء عملية التسجيل. يرجى المحاولة لاحقاً.");
                // إرجاع العرض مع بيانات النموذج الأصلية ليتمكن المستخدم من التعديل
                return View(model);
            }
        }

        /// <summary>
        /// توليد رقم موظف فريد بناءً على التاريخ والوقت الحالي
        /// </summary>
        private string GenerateEmployeeNumber()
        {
            // تنسيق: EMP-YYYYMMDD-HHMMSS-XXXX (مثال: EMP-20260817-143025-0001)
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var randomPart = new Random().Next(1000, 9999);
            return $"EMP-{timestamp}-{randomPart}";
        }
    }
}
