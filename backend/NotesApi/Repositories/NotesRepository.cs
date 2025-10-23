using Dapper;
using NotesApi.Models;
using System.Data.SqlClient;

namespace NotesApi.Repositories;

public class NotesRepository : INotesRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public NotesRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Note>> GetAllAsync(int userId, string? search = null, string? sortBy = null, string? sortOrder = null, int? page = null, int? pageSize = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"
            SELECT n.Id, n.Title, n.Content, n.CreatedAt, n.UpdatedAt, n.UserId,
                   u.Id, u.Username, u.Email, u.CreatedAt
            FROM Notes n
            INNER JOIN Users u ON n.UserId = u.Id
            WHERE n.UserId = @UserId";

        var parameters = new { UserId = userId };

        if (!string.IsNullOrEmpty(search))
        {
            sql += " AND (n.Title LIKE @Search OR n.Content LIKE @Search)";
            parameters = parameters with { Search = $"%{search}%" };
        }

        sortBy ??= "CreatedAt";
        sortOrder ??= "DESC";

        sql += $" ORDER BY n.{sortBy} {(sortOrder.ToUpper() == "ASC" ? "ASC" : "DESC")}";

        if (page.HasValue && pageSize.HasValue)
        {
            sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            parameters = parameters with { Offset = (page.Value - 1) * pageSize.Value, PageSize = pageSize.Value };
        }

        var notes = await connection.QueryAsync<Note, User, Note>(
            sql,
            (note, user) =>
            {
                note.User = user;
                return note;
            },
            parameters,
            splitOn: "Id"
        );

        return notes;
    }

    public async Task<Note?> GetByIdAsync(int id, int userId)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"
            SELECT n.Id, n.Title, n.Content, n.CreatedAt, n.UpdatedAt, n.UserId,
                   u.Id, u.Username, u.Email, u.CreatedAt
            FROM Notes n
            INNER JOIN Users u ON n.UserId = u.Id
            WHERE n.Id = @Id AND n.UserId = @UserId";

        var note = await connection.QueryAsync<Note, User, Note>(
            sql,
            (note, user) =>
            {
                note.User = user;
                return note;
            },
            new { Id = id, UserId = userId },
            splitOn: "Id"
        );

        return note.FirstOrDefault();
    }

    public async Task<Note> CreateAsync(Note note)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"
            INSERT INTO Notes (Title, Content, UserId, CreatedAt, UpdatedAt)
            OUTPUT INSERTED.*
            VALUES (@Title, @Content, @UserId, @CreatedAt, @UpdatedAt)";

        return await connection.QuerySingleAsync<Note>(sql, note);
    }

    public async Task<Note> UpdateAsync(Note note)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"
            UPDATE Notes
            SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt
            OUTPUT INSERTED.*
            WHERE Id = @Id AND UserId = @UserId";

        return await connection.QuerySingleAsync<Note>(sql, note);
    }

    public async Task DeleteAsync(int id, int userId)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId";
        await connection.ExecuteAsync(sql, new { Id = id, UserId = userId });
    }

    public async Task<int> GetTotalCountAsync(int userId, string? search = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = "SELECT COUNT(*) FROM Notes WHERE UserId = @UserId";
        var parameters = new { UserId = userId };

        if (!string.IsNullOrEmpty(search))
        {
            sql += " AND (Title LIKE @Search OR Content LIKE @Search)";
            parameters = parameters with { Search = $"%{search}%" };
        }

        return await connection.ExecuteScalarAsync<int>(sql, parameters);
    }
}