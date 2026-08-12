using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


    [ApiController]
    public class AuthController : Controller
    {
        public bool Authorized = false;
        [HttpGet("/api/")]
        public IActionResult Index()
        {
            if(Authorized)
            {
                return Ok(new{message="Ok!"});    
            } else {
            return BadRequest(new{message="You are not Authorized!"});
            }
        }

        [HttpPost("/api/authorize")]
        public IActionResult Authorize()
        {
            return Ok();
        }


    }

