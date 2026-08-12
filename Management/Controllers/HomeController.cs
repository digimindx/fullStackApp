using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Management.Models;
using CORE.Models.HR;

namespace Management.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Register 
    [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
}
