using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Models;
using NotesApi.Services;
using System.Security.Claims;

namespace NotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly NotesService _notesService;

    public NotesController(NotesService notesService)
    {
        _notesService = notesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null,
        [FromQuery] int? page = null,
        [FromQuery] int? pageSize = null)
    {
        try
        {
            var notes = await _notesService.GetAllAsync(search, sortBy, sortOrder, page, pageSize);
            var totalCount = await _notesService.GetTotalCountAsync(search);

            var response = new
            {
                notes,
                totalCount,
                page,
                pageSize
            };

            return Ok(new ApiResponse<object> { Data = response });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new ErrorResponse { Message = "Authentication required" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred retrieving notes" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var note = await _notesService.GetByIdAsync(id);
            if (note == null)
            {
                return NotFound(new ErrorResponse { Message = "Note not found" });
            }

            return Ok(new ApiResponse<Note> { Data = note });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new ErrorResponse { Message = "Authentication required" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred retrieving the note" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid request data",
                    Errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                });
            }

            var note = await _notesService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = note.Id }, new ApiResponse<Note> { Data = note });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new ErrorResponse { Message = "Authentication required" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred creating the note" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid request data",
                    Errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                });
            }

            var note = await _notesService.UpdateAsync(id, request);
            return Ok(new ApiResponse<Note> { Data = note });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new ErrorResponse { Message = "Note not found" });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new ErrorResponse { Message = "Authentication required" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred updating the note" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _notesService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new ErrorResponse { Message = "Note not found" });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new ErrorResponse { Message = "Authentication required" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred deleting the note" });
        }
    }
}