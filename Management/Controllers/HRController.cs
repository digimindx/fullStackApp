using Microsoft.AspNetCore.Mvc;


namespace Management.Controllers;

public class HRController : Controller
{
    // Index page cannot be viewed if user is not authoriezed.
    // user must login first from /Home/Login
    [HttpGet("/hr/")]
    public IActionResult Index()
    {
        return Ok("User is Authorized!");
    }
}