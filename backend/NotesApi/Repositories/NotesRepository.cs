using Dapper;
using NotesApi.Models;
using NotesApi.Data;
using Microsoft.Data.Sqlite;

namespace NotesApi.Repositories;

public class NotesRepository : INotesRepository
{
    private readonly DatabaseConnectionFactory _connectionFactory;

    public NotesRepository(DatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Note>> GetAllAsync(int userId, string? search = null, string? sortBy = null, string? sortOrder = null, int? page = null, int? pageSize = null)
    {
        using var connection = _connectionFactory.CreateSqliteConnection();

        var sql = @"
            SELECT n.Id, n.Title, n.Content, n.CreatedAt, n.UpdatedAt, n.UserId,
                   u.Id, u.Username, u.Email, u.CreatedAt
            FROM Notes n
            INNER JOIN Users u ON n.UserId = u.Id
            WHERE n.UserId = @UserId";

        object parameters;

        if (!string.IsNullOrEmpty(search) && page.HasValue && pageSize.HasValue)
        {
            sql += " AND (n.Title LIKE @Search OR n.Content LIKE @Search)";
            sql += " LIMIT @PageSize OFFSET @Offset";
            parameters = new { UserId = userId, Search = $"%{search}%", Offset = (page.Value - 1) * pageSize.Value, PageSize = pageSize.Value };
        }
        else if (!string.IsNullOrEmpty(search))
        {
            sql += " AND (n.Title LIKE @Search OR n.Content LIKE @Search)";
            parameters = new { UserId = userId, Search = $"%{search}%" };
        }
        else if (page.HasValue && pageSize.HasValue)
        {
            sql += " LIMIT @PageSize OFFSET @Offset";
            parameters = new { UserId = userId, Offset = (page.Value - 1) * pageSize.Value, PageSize = pageSize.Value };
        }
        else
        {
            parameters = new { UserId = userId };
        }

        sortBy ??= "CreatedAt";
        sortOrder ??= "DESC";
        sql += $" ORDER BY n.{sortBy} {(sortOrder.ToUpper() == "ASC" ? "ASC" : "DESC")}";

        // Note: LIMIT and OFFSET are already added in the conditional blocks above

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
        using var connection = _connectionFactory.CreateSqliteConnection();

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
        using var connection = _connectionFactory.CreateSqliteConnection();

        var sql = @"
            INSERT INTO Notes (Title, Content, UserId, CreatedAt, UpdatedAt)
            VALUES (@Title, @Content, @UserId, @CreatedAt, @UpdatedAt);
            SELECT * FROM Notes WHERE Id = last_insert_rowid();";

        return await connection.QuerySingleAsync<Note>(sql, note);
    }

    public async Task<Note> UpdateAsync(Note note)
    {
        using var connection = _connectionFactory.CreateSqliteConnection();

        var sql = @"
            UPDATE Notes
            SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt
            WHERE Id = @Id AND UserId = @UserId;
            SELECT * FROM Notes WHERE Id = @Id;";

        return await connection.QuerySingleAsync<Note>(sql, note);
    }

    public async Task DeleteAsync(int id, int userId)
    {
        using var connection = _connectionFactory.CreateSqliteConnection();

        var sql = "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId";
        await connection.ExecuteAsync(sql, new { Id = id, UserId = userId });
    }

    public async Task<int> GetTotalCountAsync(int userId, string? search = null)
    {
        using var connection = _connectionFactory.CreateSqliteConnection();

        var sql = "SELECT COUNT(*) FROM Notes WHERE UserId = @UserId";

        object parameters;
        if (!string.IsNullOrEmpty(search))
        {
            sql += " AND (Title LIKE @Search OR Content LIKE @Search)";
            parameters = new { UserId = userId, Search = $"%{search}%" };
        }
        else
        {
            parameters = new { UserId = userId };
        }

        return await connection.ExecuteScalarAsync<int>(sql, parameters);
    }
}