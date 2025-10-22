using Microsoft.AspNetCore.Mvc;
using NotesApi.Models;
using NotesApi.Repositories;

namespace NotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class NotesController : ControllerBase
{
    private readonly INotesRepository _repository;

    public NotesController(INotesRepository repository)
    {
        _repository = repository;
    }

    private string? GetUserId()
    {
        // For junior version: use header X-User-Id; Auth optional
        if (Request.Headers.TryGetValue("X-User-Id", out var values))
        {
            return values.FirstOrDefault();
        }
        return null;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotes([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] string? sortDir)
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized("Missing X-User-Id header");
        var notes = await _repository.GetNotesAsync(userId, search, sortBy, sortDir);
        return Ok(notes.Select(n => new
        {
            n.Id,
            n.Title,
            n.CreatedAt,
            n.UpdatedAt
        }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetNote(Guid id)
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized("Missing X-User-Id header");
        var note = await _repository.GetNoteAsync(userId, id);
        if (note is null) return NotFound();
        return Ok(note);
    }

    public sealed record CreateNoteRequest(string Title, string? Content);
    public sealed record UpdateNoteRequest(string Title, string? Content);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest request)
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized("Missing X-User-Id header");
        if (string.IsNullOrWhiteSpace(request.Title)) return BadRequest("Title is required");

        var note = new Note
        {
            UserId = userId,
            Title = request.Title,
            Content = string.IsNullOrWhiteSpace(request.Content) ? null : request.Content
        };
        var created = await _repository.CreateNoteAsync(note);
        return CreatedAtAction(nameof(GetNote), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNoteRequest request)
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized("Missing X-User-Id header");
        if (string.IsNullOrWhiteSpace(request.Title)) return BadRequest("Title is required");

        var existing = await _repository.GetNoteAsync(userId, id);
        if (existing is null) return NotFound();
        existing.Title = request.Title;
        existing.Content = string.IsNullOrWhiteSpace(request.Content) ? null : request.Content;
        var ok = await _repository.UpdateNoteAsync(existing);
        if (!ok) return NotFound();
        return Ok(existing);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized("Missing X-User-Id header");
        var ok = await _repository.DeleteNoteAsync(userId, id);
        if (!ok) return NotFound();
        return NoContent();
    }
}


