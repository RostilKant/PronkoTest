using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class NoteDto
    {
        public Guid Id { get; set; }
        
        [MaxLength(20, ErrorMessage = "Maximum length of the title is 20 characters")]
        public string Title { get; set; }
        
        [MaxLength(120, ErrorMessage = "Maximum length of the text is 120 characters")]
        public string Text { get; set; }
    }
}