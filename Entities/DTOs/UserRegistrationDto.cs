using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        
        public ICollection<string> Roles { get; set; }
    }
}