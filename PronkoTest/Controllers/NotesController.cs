using System;
using System.Threading.Tasks;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace PronkoTest.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users/notes")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _noteService.GetNotesAsync(HttpContext.User.Identity?.Name, ModelState);
            
            if (ModelState.ErrorCount > 0)
                return BadRequest(ModelState);
            
            return Ok(notes);
        }
        
        [HttpGet("{id}", Name = "NoteById")]
        public async Task<IActionResult> GetNoteById(Guid id)
        {
            var notes = await _noteService.GetNoteAsync(HttpContext.User.Identity?.Name, id, ModelState);
            
            if (ModelState.ErrorCount > 0)
                return BadRequest(ModelState);
            
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteManipulationDto noteManipulation)
        {
            var note = await _noteService.CreateNoteAsync(HttpContext.User.Identity?.Name, noteManipulation);
            
            return CreatedAtRoute("NoteById", new {note.Id}, note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote([FromBody] NoteManipulationDto noteManipulation, Guid id)
        {
            if (! await _noteService.UpdateNoteAsync(HttpContext.User.Identity?.Name, id, noteManipulation, ModelState))
                return BadRequest(ModelState);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            if (! await _noteService.DeleteNoteAsync(HttpContext.User.Identity?.Name, id, ModelState))
                return BadRequest(ModelState);

            return NoContent();
        }
    }
}