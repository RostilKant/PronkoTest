using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Note
    {
        [Column("NoteId")]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(20, ErrorMessage = "Maximum length of the title is 20 characters")]
        public string Title { get; set; }
        
        [MaxLength(120, ErrorMessage = "Maximum length of the text is 120 characters")]
        public string Text { get; set; }
        
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}