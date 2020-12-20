using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Services.Contracts
{
    public interface INoteService
    {
        Task<IEnumerable<NoteDto>> GetNotesAsync(string userId, ModelStateDictionary modelState);

        Task<NoteDto> GetNoteAsync(string userId, Guid noteId, ModelStateDictionary modelState);

        Task<NoteDto> CreateNoteAsync(string userId, NoteManipulationDto noteManipulation);

        Task<bool> UpdateNoteAsync(string userId, Guid noteId, NoteManipulationDto noteManipulation,
            ModelStateDictionary modelState);

        Task<bool> DeleteNoteAsync(string userId, Guid noteId, ModelStateDictionary modelState);
    }
}