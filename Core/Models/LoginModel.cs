using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username or Email is required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}