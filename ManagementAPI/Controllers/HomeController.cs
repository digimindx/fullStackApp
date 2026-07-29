using Microsoft.AspNetCore.Mvc;

namespace ManagementAPI.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/management/api/")]
        public IActionResult Index()
        {
            return Ok(new {status =  true, message = "Running!" });
        }
    }
}
