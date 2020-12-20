using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Entities.Models;

namespace Repository.Contracts
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllNotesAsync(string userId, NoteParameters noteParameters, bool trackChanges);

        Task<Note> GetNoteByIdAsync(string userId, Guid noteId, bool trackChanges);

        void CreateNote(Note note);

        void DeleteNote(Note note);
    }
}