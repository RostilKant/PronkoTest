using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        public string FirstName { get; set; } 
        
        [Required]
        public string LastName { get; set; }
    }
}