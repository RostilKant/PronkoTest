using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class NoteService : INoteService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<NoteService> _logger;
        private readonly IMapper _mapper;

        public NoteService(IRepositoryManager repositoryManager, ILogger<NoteService> logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NoteDto>> GetNotesAsync(string userId, NoteParameters noteParameters, ModelStateDictionary modelState)
        {
            var notes = await _repositoryManager.Note.GetAllNotesAsync(userId, noteParameters, false);
            if (notes == null)
            {
                _logger.Log(LogLevel.Error, "No notes!");
                modelState.TryAddModelError("empty-notes", "User hadn't been yet created any note");
            }

            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }

        public async Task<NoteDto> GetNoteAsync(string userId, Guid noteId, ModelStateDictionary modelState)
        {
            var note = await _repositoryManager.Note.GetNoteByIdAsync(userId, noteId, false);

            if (note == null)
            {
                _logger.Log(LogLevel.Error, "Note with such id doesn't exists!");
                modelState.TryAddModelError("invalid-note-id", "Note with such id doesn't exists!");
                return null;
            }

            return _mapper.Map<NoteDto>(note);
        }

        public async Task<NoteDto> CreateNoteAsync(string userId, NoteManipulationDto noteManipulation)
        {
            var note = _mapper.Map<Note>(noteManipulation);
            note.UserId = userId;
            _repositoryManager.Note.CreateNote(note);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<NoteDto>(note);
        }
        
        public async Task<bool> UpdateNoteAsync(string userId, Guid noteId, NoteManipulationDto noteManipulation, 
            ModelStateDictionary modelState)
        {
            var note = await _repositoryManager.Note.GetNoteByIdAsync(userId, noteId, true);
            
            if (note == null)
            {
                _logger.Log(LogLevel.Error, "Note with such id doesn't exists!");
                modelState.TryAddModelError("invalid-note-id", "Note with such id doesn't exists!");
                return false;
            }

            _mapper.Map(noteManipulation, note);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteNoteAsync(string userId, Guid noteId, ModelStateDictionary modelState)
        {
            var note = await _repositoryManager.Note.GetNoteByIdAsync(userId, noteId, false);
            
            if (note == null)
            {
                _logger.Log(LogLevel.Error, "Note with such id doesn't exists!");
                modelState.TryAddModelError("invalid-note-id", "Note with such id doesn't exists!");
                return false;
            }
            
            _repositoryManager.Note.DeleteNote(note);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}