using Microsoft.AspNetCore.Mvc;

namespace ClientAPI.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/client/api/")]
        public IActionResult Index()
        {
            return Ok("running /client/api/");
        }
    }
}
