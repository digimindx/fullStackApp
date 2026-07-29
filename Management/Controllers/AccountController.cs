using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Core.Interfaces;
using Core.Models;

namespace Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmployee _employeeRepository;

        public AccountController(IEmployee employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _employeeRepository.RegisterAsync(model);
            if (success)
            {
                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Username or Email already exists.");
            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var employee = await _employeeRepository.AuthenticateAsync(model);
            if (employee != null)
            {
                // Create user identity claims for the session cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, employee.ID.ToString()),
                    new Claim(ClaimTypes.Name, employee.Username ?? employee.FirstName),
                    new Claim(ClaimTypes.Email, employee.Email ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your credentials.");
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}