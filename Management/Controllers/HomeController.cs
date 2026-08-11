using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Management.Models;
using CORE.Entities;

namespace Management.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        Employee emp = new Employee();
        return Ok(new{message = emp });
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
}
