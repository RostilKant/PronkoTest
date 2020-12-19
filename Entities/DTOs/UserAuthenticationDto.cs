using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class UserAuthenticationDto
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}