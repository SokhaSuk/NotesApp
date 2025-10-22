using Dapper;
using NotesApi.Data;
using NotesApi.Models;
using System.Text;

namespace NotesApi.Repositories;

public sealed class NotesRepository : INotesRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public NotesRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Note>> GetNotesAsync(string userId, string? search, string? sortBy, string? sortDir)
    {
        var builder = new StringBuilder();
        builder.Append("SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt FROM dbo.Notes WHERE UserId = @UserId ");
        if (!string.IsNullOrWhiteSpace(search))
        {
            builder.Append("AND (Title LIKE @Search OR Content LIKE @Search) ");
        }

        var safeSortBy = sortBy?.ToLowerInvariant() switch
        {
            "title" => "Title",
            "createdat" => "CreatedAt",
            "updatedat" => "UpdatedAt",
            _ => "CreatedAt"
        };
        var direction = string.Equals(sortDir, "asc", StringComparison.OrdinalIgnoreCase) ? "ASC" : "DESC";
        builder.Append($"ORDER BY {safeSortBy} {direction}");

        var sql = builder.ToString();
        await using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Note>(sql, new { UserId = userId, Search = $"%{search}%" });
    }

    public async Task<Note?> GetNoteAsync(string userId, Guid id)
    {
        const string sql = "SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt FROM dbo.Notes WHERE Id = @Id AND UserId = @UserId";
        await using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Note>(sql, new { Id = id, UserId = userId });
    }

    public async Task<Note> CreateNoteAsync(Note note)
    {
        note.Id = note.Id == Guid.Empty ? Guid.NewGuid() : note.Id;
        var now = DateTime.UtcNow;
        note.CreatedAt = now;
        note.UpdatedAt = now;
        const string sql = @"
INSERT INTO dbo.Notes (Id, UserId, Title, Content, CreatedAt, UpdatedAt)
VALUES (@Id, @UserId, @Title, @Content, @CreatedAt, @UpdatedAt);
";
        await using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, note);
        return note;
    }

    public async Task<bool> UpdateNoteAsync(Note note)
    {
        note.UpdatedAt = DateTime.UtcNow;
        const string sql = @"
UPDATE dbo.Notes
SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt
WHERE Id = @Id AND UserId = @UserId;
";
        await using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, note);
        return rows > 0;
    }

    public async Task<bool> DeleteNoteAsync(string userId, Guid id)
    {
        const string sql = "DELETE FROM dbo.Notes WHERE Id = @Id AND UserId = @UserId";
        await using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, new { Id = id, UserId = userId });
        return rows > 0;
    }
}


