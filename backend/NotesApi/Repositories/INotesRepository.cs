using NotesApi.Models;

namespace NotesApi.Repositories;

public interface INotesRepository
{
    Task<IEnumerable<Note>> GetAllAsync(int userId, string? search = null, string? sortBy = null, string? sortOrder = null, int? page = null, int? pageSize = null);
    Task<Note?> GetByIdAsync(int id, int userId);
    Task<Note> CreateAsync(Note note);
    Task<Note> UpdateAsync(Note note);
    Task DeleteAsync(int id, int userId);
    Task<int> GetTotalCountAsync(int userId, string? search = null);
}