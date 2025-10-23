using NotesApi.Models;
using NotesApi.Repositories;

namespace NotesApi.Services;

public class NotesService
{
    private readonly INotesRepository _notesRepository;
    private readonly IUserContext _userContext;

    public NotesService(INotesRepository notesRepository, IUserContext userContext)
    {
        _notesRepository = notesRepository;
        _userContext = userContext;
    }

    public async Task<IEnumerable<Note>> GetAllAsync(string? search = null, string? sortBy = null, string? sortOrder = null, int? page = null, int? pageSize = null)
    {
        var userId = _userContext.GetUserId();
        return await _notesRepository.GetAllAsync(userId, search, sortBy, sortOrder, page, pageSize);
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        var userId = _userContext.GetUserId();
        return await _notesRepository.GetByIdAsync(id, userId);
    }

    public async Task<Note> CreateAsync(CreateNoteRequest request)
    {
        var userId = _userContext.GetUserId();
        var note = new Note
        {
            Title = request.Title,
            Content = request.Content,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _notesRepository.CreateAsync(note);
    }

    public async Task<Note> UpdateAsync(int id, UpdateNoteRequest request)
    {
        var userId = _userContext.GetUserId();
        var existingNote = await _notesRepository.GetByIdAsync(id, userId);

        if (existingNote == null)
        {
            throw new KeyNotFoundException("Note not found");
        }

        existingNote.Title = request.Title;
        existingNote.Content = request.Content;
        existingNote.UpdatedAt = DateTime.UtcNow;

        return await _notesRepository.UpdateAsync(existingNote);
    }

    public async Task DeleteAsync(int id)
    {
        var userId = _userContext.GetUserId();
        await _notesRepository.DeleteAsync(id, userId);
    }

    public async Task<int> GetTotalCountAsync(string? search = null)
    {
        var userId = _userContext.GetUserId();
        return await _notesRepository.GetTotalCountAsync(userId, search);
    }
}
