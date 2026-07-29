using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Core.Interfaces;
using Core.Models;

namespace ClientAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IEmployee _employeeRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IEmployee employeeRepository, IConfiguration configuration)
        {
            _employeeRepository = employeeRepository;
            _configuration = configuration;
        }

        // POST: api/auth/register
        [HttpPost("api/auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _employeeRepository.RegisterAsync(model);
            if (!success)
            {
                return BadRequest(new { message = "Username or Email already exists." });
            }

            return Ok(new { message = "Registration successful!" });
        }

        // POST: api/auth/login
        [HttpPost("/api/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeRepository.AuthenticateAsync(model);
            if (employee == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            // Generate JWT Token for React Frontend
            var token = GenerateJwtToken(employee.Username ?? employee.FirstName, employee.Email ?? "");

            return Ok(new
            {
                token = token,
                username = employee.Username,
                message = "Login successful!"
            });
        }

        private string GenerateJwtToken(string username, string email)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3), // Token valid for 3 hours
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}