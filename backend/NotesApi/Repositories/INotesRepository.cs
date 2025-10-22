using NotesApi.Models;

namespace NotesApi.Repositories;

public interface INotesRepository
{
    Task<IEnumerable<Note>> GetNotesAsync(string userId, string? search, string? sortBy, string? sortDir);
    Task<Note?> GetNoteAsync(string userId, Guid id);
    Task<Note> CreateNoteAsync(Note note);
    Task<bool> UpdateNoteAsync(Note note);
    Task<bool> DeleteNoteAsync(string userId, Guid id);
}


