using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository
{
    public class NoteRepository: RepositoryBase<Note>, INoteRepository
    {
        public NoteRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        { }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(string userId, NoteParameters noteParameters, bool trackChanges) =>
            await FindByCondition(x => x.UserId == userId, trackChanges)
                .Skip((noteParameters.PageNumber - 1) * noteParameters.PageSize)
                .Take(noteParameters.PageSize)
                .ToListAsync();

        public async Task<Note> GetNoteByIdAsync(string userId, Guid noteId, bool trackChanges) =>
            await FindByCondition(x => x.UserId.Equals(userId) && x.Id.Equals(noteId), 
                    trackChanges)
                .SingleOrDefaultAsync();

        public void CreateNote(Note note) => Create(note);
        public void DeleteNote(Note note) => Delete(note);
    }
}